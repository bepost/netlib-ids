namespace Fujiberg.Coder.CSharp;

public sealed record Namespace
{
    public static readonly Namespace Global = "";

    public Namespace(string name)
    {
        Name = name;
    }

    public string Name { get; init; }

    public string ToCode(bool useGlobalPrefix = true, bool includeTrailingDot = false)
    {
        if (this == Global)
            return includeTrailingDot ? "global::" : "global";
        return (useGlobalPrefix ? "global::" : "") + Name + (includeTrailingDot ? "." : "");
    }

    public static QualifiedName operator +(Namespace ns, string name)
    {
        return new QualifiedName(ns, name);
    }

    public static implicit operator Namespace(string name)
    {
        return new Namespace(name);
    }
}
