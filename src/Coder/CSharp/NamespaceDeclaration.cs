using System.Collections.Immutable;

namespace Fujiberg.Coder.CSharp;

public sealed record NamespaceDeclaration : NamedDeclaration
{
    public NamespaceDeclaration(Namespace @namespace) : base(@namespace.ToCode(useGlobalPrefix:false))
    {
    }

    public NamespaceDeclaration(string name) : base(name)
    {
    }

    public IImmutableSet<NamedDeclaration> Declarations { get; init; } = [];

    public override string ToCode()
    {
        var cw = new CodeWriter();
        cw.WriteLine($"namespace {Name}");
        using (cw.Block())
        {
            foreach (var ns in Declarations)
                cw.WriteLine(ns.ToCode());
        }

        return cw.ToString();
    }
}
