using System;
using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record Attribution
{
    public Attribution(QualifiedName type)
    {
        if (type.Name.EndsWith("Attribute", StringComparison.Ordinal))
            type = type with {Name = type.Name[..^9]};
        Type = type;
    }

    public IImmutableList<Expression> Arguments { get; init; } = [];
    public QualifiedName Type { get; init; }

    public string ToCode()
    {
        var cw = new CodeWriter();
        cw.Write("[");
        cw.Write(Type.ToCode());
        if (Arguments.Any())
        {
            cw.Write("(");
            cw.Write(string.Join(", ", Arguments.Select(x => x.ToCode())));
            cw.Write(")");
        }

        cw.Write("]");
        return cw.ToString();
    }
}
