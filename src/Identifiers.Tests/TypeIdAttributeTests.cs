using System.Threading.Tasks;
using Fujiberg.Identifiers;
using Xunit;

namespace Identifiers.Tests;

public sealed class TypeIdAttributeTests
{
    public TypeIdAttributeTests()
    {
        SourceHelpers.Repeatable = true;
    }

    [Fact]
    public async Task GeneratesTypeIdAttributeCorrectly()
    {
        await SourceGeneratorTestHelpers.TestSourceGeneratorsAsync(
            [new TypedIdAttributeGenerator()],
            [
                """
                using Fujiberg.Identifiers;
                namespace MyNamespace
                {
                    [TypedIdAttribute]
                    public partial record struct SomeId;
                }
                """
            ]
        );
    }

    [Fact]
    public async Task GeneratesTypeIdImplementationCorrectly()
    {
        await SourceGeneratorTestHelpers.TestSourceGeneratorsAsync(
            [
                new TypedIdAttributeGenerator(),
                new TypedIdImplementationGenerator()
            ],
            [
                """
                using Fujiberg.Identifiers;
                namespace MyNamespace
                {
                    [TypedIdAttribute]
                    public partial record struct SomeId;
                }
                """
            ],
            [
                // Covered by other tests
                "TypedIdAttribute.g.cs"
            ]
        );
    }
}
