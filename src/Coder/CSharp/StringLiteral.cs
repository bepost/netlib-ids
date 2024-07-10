namespace Fujiberg.Coder.CSharp;

public sealed record StringLiteral : Literal
{
    public StringLiteral(string value)
    {
        Value = value;
    }

    public string Value { get; init; }

    public override string ToCode()
    {
        var value = Value.Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\0", "\\0");
        return $"\"{value}\"";
    }
}
