using System.Collections.Immutable;

namespace Fujiberg.Coder.CSharp;

public abstract record TypeDeclaration : NamedDeclaration
{
    protected TypeDeclaration(string name) : base(name)
    {
    }

    public Accessor Accessor { get; init; } = Accessor.Internal;
    public IImmutableSet<Attribution> Attributes { get; set; } = [];
}
