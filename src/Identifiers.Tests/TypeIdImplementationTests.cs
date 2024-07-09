using System.Threading.Tasks;
using Fujiberg.Identifiers;
using Xunit;

namespace Identifiers.Tests;

public sealed class TypeIdImplementationTests
{
    public TypeIdImplementationTests()
    {
        SourceHelpers.Repeatable = true;
    }

    [Fact]
    public async Task GenerateCorrectly()
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
