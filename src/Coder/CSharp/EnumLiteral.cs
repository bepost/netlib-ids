namespace Fujiberg.Coder.CSharp;

public sealed record EnumLiteral(QualifiedName Name, string Value) : Literal
{
    public static EnumLiteral Create(QualifiedName name, string value)
    {
        return new EnumLiteral(name, value);
    }

    public override string ToCode()
    {
        return $"{Name.ToCode()}.{Value}";
    }
}
