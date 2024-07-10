using System.Collections.Immutable;

namespace Fujiberg.Coder.CSharp;

public abstract record ObjectDeclaration : TypeDeclaration
{
    private protected ObjectDeclaration(string name) : base(name)
    {
    }

    public IImmutableSet<QualifiedName> Interfaces { get; init; } = [];
    public bool IsPartial { get; init; }
}
