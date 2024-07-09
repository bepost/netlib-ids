using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyXunit;

namespace Identifiers.Tests;

internal static class SourceGeneratorTestHelpers
{
    public static async Task TestSourceGeneratorsAsync(
        IEnumerable<IIncrementalGenerator> generators,
        IEnumerable<string> sources,
        IEnumerable<string>? ignoreFiles = null)
    {
        ignoreFiles ??= [];

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators.ToArray());

        var compilation = CSharpCompilation.Create("Tests", sources.Select(s => CSharpSyntaxTree.ParseText(s)));
        driver = driver.RunGenerators(compilation);

        var results = driver.GetRunResult()
            .Results.SelectMany(r => r.GeneratedSources)
            .Where(x => !ignoreFiles.Contains(x.HintName))
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

        await Verifier.Verify(string.Concat(results));
    }
}
