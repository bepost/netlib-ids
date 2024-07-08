using System;
using System.Linq;
using Bogus;
using FluentAssertions;
using Fujiberg.Coder.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Coder.Tests;

public sealed class CodeFileTests
{
    public CodeFileTests(ITestOutputHelper log)
    {
        Log = log;
    }

    public ITestOutputHelper Log { get; }
    private static readonly Faker Faker = new();

    [Fact]
    public void ShouldWriteMultilineHeaderCorrectly()
    {
        // Arrange
        var lines = new[] {Faker.Lorem.Sentence(), Faker.Lorem.Sentence(), Faker.Lorem.Sentence()};
        var header = string.Join('\n', lines);
        var expected = string.Concat(lines.Select(l => $"// {l}{Environment.NewLine}"));

        // Act
        var cf = new CodeFile {HeaderComment = header};
        var output = cf.ToCode();

        Log.WriteLine(output);

        // Assert
        output.Should()
            .StartWith(expected);
    }
}
