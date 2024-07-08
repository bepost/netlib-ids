using System;
using System.Collections.Immutable;

namespace Fujiberg.Coder.CSharp;

public abstract record Declaration
{
    public Accessor Accessor { get; init; } = Accessor.Internal;
    public IImmutableSet<Attribution> Attributes { get; init; } = [];
    public required string Name { get; init; }
    public required Namespace Namespace { get; init; }
    public abstract string ToCode(CodeGenContext context);
}

public sealed record DelegateDeclaration : Declaration
{
    public DelegateDeclaration()
    {
        throw new NotImplementedException();
    }

    // TODO
    public override string ToCode(CodeGenContext context)
    {
        throw new NotImplementedException();
    }
}
