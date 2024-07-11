using System;
using System.Collections.Immutable;
using System.Text;
using Fujiberg.Coder.CSharp;
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

    cf.ToCode();

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
                SourceHelpers.GetOutputFileName(typedId.QualifiedName),
                SourceText.From(source, Encoding.UTF8)
            );
        }
    }

    private static string GenerateTypeIdSource(TypedIdDeclaration typedId)
    {
        var cf = new CodeFile
        {
            Headers = [SourceHelpers.HeaderComment],
            Declarations =
            [
                new NamespaceDeclaration(typedId.NameSpace)
                {
                    Declarations =
                    [
                        new RecordStructObjectDeclaration(typedId.Name)
                        {
                            Partial = true,
                            Interfaces =
                                [new QualifiedName("System", "IParsable") {Generics = [typedId.QualifiedName]}],
                            Attributes = [SourceHelpers.GeneratedAttribution],
                            Fields =
                            [
                                new Field(typeof(Guid), "_value") {Accessor = Accessor.Private, ReadOnly = true}
                            ],
                            Constructors =
                            [
                                Constructor.Create(typedId.Name)
                                    .Public()
                                    .WithParameters(Parameter.Create("System.Guid", "value"))
                                    .WithBody("this._value = value;")
                            ],
                            Properties =
                            [
                                new Property(typeof(Guid), "Value")
                                {
                                    Accessor = Accessor.Public, Getter = new Getter {Body = "return _value;"}
                                }
                            ],
                            Methods =
                            [
                                new Method(typedId.QualifiedName, "New")
                                {
                                    Accessor = Accessor.Public,
                                    Static = true,
                                    Body = $"return new {typedId.Name}(global::System.Guid.NewGuid());"
                                },
                                new Method(typeof(string), "ToString")
                                {
                                    Accessor = Accessor.Public, Override = true, Body = "return $\"{_value}\";"
                                },
                                new Method(typedId.QualifiedName, "Parse")
                                {
                                    Accessor = Accessor.Public,
                                    Static = true,
                                    Parameters =
                                    [
                                        Parameter.Create(typeof(string), "s"),
                                        Parameter.Create("IFormatProvider?", "provider")
                                    ],
                                    Body = $"return new {typedId.Name}(global::System.Guid.Parse(s));"
                                },
                                new Method(typeof(bool), "TryParse")
                                {
                                    Accessor = Accessor.Public,
                                    Static = true,
                                    Parameters =
                                    [
                                        Parameter.Create("string", "s")
                                            .AddAttributes(
                                                Attribution.Create(
                                                    "System.Diagnostics.CodeAnalysis.NotNullWhen",
                                                    BoolLiteral.True
                                                )
                                            ),
                                        Parameter.Create("global::System.IFormatProvider?", "provider"),
                                        Parameter.Create(typedId.QualifiedName, "result")
                                            .Out()
                                            .AddAttributes(
                                                Attribution.Create(
                                                    "System.Diagnostics.CodeAnalysis.MaybeNullWhen",
                                                    BoolLiteral.False
                                                )
                                            )
                                    ],
                                    Body = $$"""
                                             if(!global::System.Guid.TryParse(s, out var guid))
                                             {
                                                 result = default;
                                                 return false;
                                             }

                                             result = new {{typedId.NameSpace}}.{{typedId.Name}}(guid);
                                             return true;
                                             """
                                }
                            ]
                        }
                    ]
                }
            ]
        };
        return cf.ToCode();
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

    internal sealed class TypedIdDeclaration
    {
        public TypedIdDeclaration(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }

        public string Name { get; }
        public Namespace NameSpace { get; }
        public QualifiedName QualifiedName => NameSpace + Name;
    }
}
