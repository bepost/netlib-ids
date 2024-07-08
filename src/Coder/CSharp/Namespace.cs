namespace Fujiberg.Coder.CSharp;

public sealed record Namespace(string Name = "")
{
    public static readonly Namespace Global = new();

    public string ToCode(CodeGenContext context, bool includeTrailingDot)
    {
        if (this == Global)
            return includeTrailingDot ? "global::" : "global";
        if (context.IncludeGlobal)
            return includeTrailingDot ? $"global::{Name}." : $"global::{Name}";
        return includeTrailingDot ? $"{Name}." : Name;
    }

    public static QualifiedName operator +(Namespace ns, string name)
    {
        return new QualifiedName {Namespace = ns, Name = name};
    }
}
