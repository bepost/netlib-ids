using System.Text;
using Fujiberg.Coder.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Fujiberg.Identifiers;

[Generator]
public sealed class TypedIdAttributeGenerator : IIncrementalGenerator
{
    private static readonly Namespace Ns = new("Fujiberg.Identifiers");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(
            static ctx => ctx.AddSource("TypedIdAttribute.g.cs", SourceText.From(BuildFile(), Encoding.UTF8))
        );
    }

    private static string BuildFile()
    {
        var cf = new CodeFile {HeaderComment = SourceHelpers.HeaderComment}.WithDeclaration(BuildTypeIdAttribute());
        return cf.ToCode();
    }

    private static TypeDeclaration BuildTypeIdAttribute()
    {
        var cd = TypeDeclaration.Class(Ns + "TypedIdAttribute")
            .Public()
            .Sealed()
            .WithParent("System.Attribute")
            .WithAttribute(
                Attribution.Create("System.AttributeUsage", EnumLiteral.Create("System.AttributeTargets", "Struct"))
            )
            .WithAttribute(SourceHelpers.GeneratedAttribution);
        return cd;
    }
}
