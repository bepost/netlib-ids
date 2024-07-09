namespace Fujiberg.Coder.CSharp;

public sealed record Namespace
{
    public static readonly Namespace Global = "";
    public required string Name { get; init; }

    public static Namespace Create(string name)
    {
        return new Namespace {Name = name};
    }

    public string ToCode(bool useGlobalPrefix = true, bool includeTrailingDot = false)
    {
        if (this == Global)
            return includeTrailingDot ? "global::" : "global";
        return (useGlobalPrefix ? "global::" : "") + Name + (includeTrailingDot ? "." : "");
    }

    public static QualifiedName operator +(Namespace ns, string name)
    {
        return new QualifiedName {Namespace = ns, Name = name};
    }

    public static implicit operator Namespace(string name)
    {
        return Create(name);
    }
}
