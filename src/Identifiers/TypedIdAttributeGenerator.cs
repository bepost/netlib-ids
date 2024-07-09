using System.Text;
using Fujiberg.Coder.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Fujiberg.Identifiers;

[Generator]
public sealed class TypedIdAttributeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(
            static ctx => ctx.AddSource("TypedIdAttribute.g.cs", SourceText.From(BuildFile(), Encoding.UTF8))
        );
    }

    private static string BuildFile()
    {
        var cf = CodeFile.Create()
            .AddHeaders(SourceHelpers.HeaderComment)
            .AddNamespaces(
                NamespaceDeclaration.Create("Fujiberg.Identifiers")
                    .AddTypes(
                        TypeDeclaration.CreateClass("TypedIdAttribute")
                            .AsPublic()
                            .AsSealed()
                            .WithParent("System.Attribute")
                            .AddAttributes(
                                Attribution.Create(
                                    "System.AttributeUsage",
                                    EnumLiteral.Create("System.AttributeTargets", "Struct")
                                ),
                                SourceHelpers.GeneratedAttribution
                            )
                    )
            );

        return cf.ToCode();
    }
}
