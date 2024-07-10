using System;
using System.Text;
using Fujiberg.Coder.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Fujiberg.Identifiers;

[Generator]
public sealed class TypedIdAttributeGenerator : IIncrementalGenerator
{
    internal static readonly QualifiedName Name = "Fujiberg.Identifiers.TypedIdAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(
            static ctx => ctx.AddSource(
                SourceHelpers.GetOutputFileName(Name),
                SourceText.From(BuildFile(), Encoding.UTF8)
            )
        );
    }

    private static string BuildFile()
    {
        var cf = new CodeFile
        {
            Headers = [SourceHelpers.HeaderComment],
            Declarations =
            [
                new NamespaceDeclaration(Name.Namespace)
                {
                    Declarations =
                    [
                        new ClassDeclaration(Name.Name)
                        {
                            Accessor = Accessor.Public,
                            IsSealed = true,
                            Parent = typeof(Attribute),
                            Attributes =
                            [
                                new Attribution(typeof(AttributeUsageAttribute))
                                {
                                    Arguments = [new EnumLiteral(AttributeTargets.Struct)]
                                },
                                SourceHelpers.GeneratedAttribution
                            ]
                        }
                    ]
                }
            ]
        };

        return cf.ToCode();
    }
}
