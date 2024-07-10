using System.Threading.Tasks;
using Fujiberg.Identifiers;
using Xunit;
using Xunit.Abstractions;

namespace Identifiers.Tests;

public sealed class TypeIdImplementationTests
{
    public TypeIdImplementationTests(ITestOutputHelper output)
    {
        _output = output;
        SourceHelpers.Repeatable = true;
    }

    private readonly ITestOutputHelper _output;

    [Fact]
    public async Task GenerateCorrectly()
    {
        await SourceGeneratorTestHelpers.TestSourceGeneratorsAsync(
            _output,
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
                SourceHelpers.GetOutputFileName(TypedIdAttributeGenerator.Name)
            ]
        );
    }
}
