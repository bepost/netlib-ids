using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Fujiberg.Identifiers;

[Generator]
public sealed class TypedIdImplementationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
                "Fujiberg.Identifiers.TypedIdAttribute",
                static (node, _) => node is RecordDeclarationSyntax,
                static (ctx, _) => GetSemanticTargetForGeneration(ctx)
            )
            .Where(static r => r is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(compilation, Execute);
    }

    private void Execute(
        SourceProductionContext context,
        (Compilation Left, ImmutableArray<TypedIdDeclaration?> Right) action)
    {
        var (_, typedIds) = action;

        foreach (var typedId in typedIds)
        {
            if (typedId == null)
                continue;

            var source = GenerateTypeIdSource(typedId);
            context.AddSource(
                SourceHelpers.GetOutputFileName(typedId.NameSpace, typedId.Name),
                SourceText.From(source, Encoding.UTF8)
            );
        }
    }

    private static string GenerateTypeIdSource(TypedIdDeclaration typedId)
    {
        return $$"""
                 {{SourceHelpers.Header}}

                 namespace {{typedId.NameSpace}};

                 {{SourceHelpers.GeneratedAttribute}}
                 partial record struct {{typedId.Name}} : global::System.IParsable<{{typedId.NameSpace}}.{{typedId.Name}}>
                 {
                     private readonly global::System.Guid _value;
                     public {{typedId.Name}}(global::System.Guid value)
                     {
                        _value = value;
                     }
                     
                     public global::System.Guid Value => _value;
                     
                     public static {{typedId.NameSpace}}.{{typedId.Name}} New() => new {{typedId.Name}}(global::System.Guid.NewGuid());
                     
                     public override string ToString() => $"{_value}";
                     
                     public static {{typedId.NameSpace}}.{{typedId.Name}} Parse(string s, global::System.IFormatProvider? provider)
                     {
                        return new {{typedId.NameSpace}}.{{typedId.Name}}(global::System.Guid.Parse(s));
                     }
                 
                     public static bool TryParse(
                        [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s,
                        global::System.IFormatProvider? provider,
                        [global::System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out {{typedId.NameSpace}}.{{typedId.Name}} result)
                     {
                        if(!global::System.Guid.TryParse(s, out var guid))
                        {
                            result = default;
                            return false;
                        }
                 
                        result = new {{typedId.NameSpace}}.{{typedId.Name}}(guid);
                        return true;
                     }
                 }
                 """;
    }

    private static string GetNameSpace(TypeDeclarationSyntax structSymbol)
    {
        // determine the namespace the struct is declared in, if any
        var potentialNamespaceParent = structSymbol.Parent;
        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax &&
               potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            potentialNamespaceParent = potentialNamespaceParent.Parent;

        if (potentialNamespaceParent is not BaseNamespaceDeclarationSyntax namespaceParent)
            return string.Empty;

        // TODO Refactor for clarity
        var nameSpace = namespaceParent.Name.ToString();
        while (true)
        {
            if (namespaceParent.Parent is not NamespaceDeclarationSyntax namespaceParentParent)
                break;

            namespaceParent = namespaceParentParent;
            nameSpace = $"{namespaceParent.Name}.{nameSpace}";
        }

        return nameSpace;
    }

    private static TypedIdDeclaration? GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext ctx)
    {
        // Asserted: has a TypedIdAttribute

        if (ctx.TargetNode is not RecordDeclarationSyntax syntax)
            return null;

        if (!syntax.IsKind(SyntaxKind.RecordStructDeclaration))
            return null;

        if (ctx.TargetSymbol is not INamedTypeSymbol symbol)
            return null;

        if (ctx.TargetNode.Parent is TypeDeclarationSyntax)
            return null;

        var nameSpace = GetNameSpace(syntax);
        var name = symbol.Name;
        return new TypedIdDeclaration(name, nameSpace);
    }

    private sealed class TypedIdDeclaration
    {
        public TypedIdDeclaration(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }

        public string Name { get; }
        public string NameSpace { get; }
    }
}
