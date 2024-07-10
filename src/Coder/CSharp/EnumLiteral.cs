using System;

namespace Fujiberg.Coder.CSharp;

public sealed record EnumLiteral : Literal
{
    public EnumLiteral(QualifiedName type, string value)
    {
        Type = type;
        Value = value;
    }

    public EnumLiteral(Enum value) : this(value.GetType(), value.ToString())
    {
    }

    public QualifiedName Type { get; init; }
    public string Value { get; init; }

    public override string ToCode()
    {
        return $"{Type.ToCode()}.{Value}";
    }
}
