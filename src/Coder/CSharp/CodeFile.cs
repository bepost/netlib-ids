using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record CodeFile
{
    public IImmutableSet<NamedDeclaration> Declarations { get; init; } = [];
    public bool EnableNullability { get; init; } = true;
    public IImmutableList<Comment> Headers { get; init; } = [];

    public string ToCode()
    {
        var cw = new CodeWriter();

        foreach (var comment in Headers)
            cw.Write(comment.ToCode());
        cw.WriteLine();

        if (EnableNullability)
            cw.WriteLine("#nullable enable");
        cw.WriteLine();

        foreach (var ns in Declarations.OrderBy(x => x.Name))
            cw.WriteLine(ns.ToCode());

        return cw.ToString();
    }
}
