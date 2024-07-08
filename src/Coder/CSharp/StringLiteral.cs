namespace Fujiberg.Coder.CSharp;

public sealed record StringLiteral(string Value) : Literal
{
    public static StringLiteral Create(string value)
    {
        return new StringLiteral(value);
    }

    public override string ToCode(CodeGenContext context)
    {
        var value = Value.Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\0", "\\0");
        return $"\"{value}\"";
    }
}
