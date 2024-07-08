using System.Linq;
using System.Threading.Tasks;
using Fujiberg.Identifiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyXunit;
using Xunit;

namespace Identifiers.Tests;

public sealed class TypeIdAttributeTests
{
    public TypeIdAttributeTests()
    {
        SourceHelpers.Repeatable = true;
    }

    [Fact]
    public Task GeneratesTypeIdAttributeCorrectly()
    {
        // The source code to test
        var source = """
                     using Fujiberg.Identifiers;
                     namespace MyNamespace
                     {
                         [TypedIdAttribute]
                         public partial record struct SomeId;
                     }
                     """;
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var compilation = CSharpCompilation.Create("Tests", [syntaxTree]);

        var generator = new TypedIdAttributeGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGenerators(compilation);

        var results = driver.GetRunResult()
            .Results.SelectMany(r => r.GeneratedSources)
            .OrderBy(x => x.HintName)
            .Select(
                x => string.Concat(
                    "==================== Start of ",
                    x.HintName,
                    " ====================\n",
                    x.SourceText,
                    "\n==================== End of ",
                    x.HintName,
                    " ====================\n\n"
                )
            );

        return Verifier.Verify(string.Concat(results));
    }
}
