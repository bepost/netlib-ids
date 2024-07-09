using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record CodeFile
{
    public bool EnableNullability { get; init; } = true;
    public IImmutableList<Comment> HeaderComment { get; init; } = [];
    public IImmutableSet<NamespaceDeclaration> NamespaceDeclarations { get; init; } = [];

    public CodeFile AddHeaders(params Comment[] headerComment)
    {
        return this with {HeaderComment = [..HeaderComment, ..headerComment]};
    }

    public CodeFile AddNamespaces(params NamespaceDeclaration[] namespaces)
    {
        return this with {NamespaceDeclarations = [..NamespaceDeclarations, ..namespaces]};
    }

    public static CodeFile Create()
    {
        return new CodeFile();
    }

    public string ToCode()
    {
        var cw = new CodeWriter();

        foreach (var comment in HeaderComment)
            cw.Write(comment.ToCode());
        cw.WriteLine();

        if (EnableNullability)
            cw.WriteLine("#nullable enable");
        cw.WriteLine();

        foreach (var ns in NamespaceDeclarations.OrderBy(x => x.Name.Name))
            cw.WriteLine(ns.ToCode());

        return cw.ToString();
    }
}
