using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Fujiberg.Identifiers;

[Generator]
public sealed class TypedIdAttributeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(
            static ctx => ctx.AddSource("TypedIdAttribute.g.cs", SourceText.From(WriteAttribute(), Encoding.UTF8))
        );
    }

    private static string WriteAttribute()
    {
        return $$"""
                 {{SourceHelpers.Header}}
                 namespace Fujiberg.Identifiers;

                 [System.AttributeUsage(System.AttributeTargets.Struct)]
                 public sealed class TypedIdAttribute : System.Attribute
                 {
                 }
                 """;
    }
}
