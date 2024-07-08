namespace Fujiberg.Coder.CSharp;

#pragma warning disable CA1847

public sealed record Comment(string Text, bool BlockMode = false)
{
    public string ToCode()
    {
        if (BlockMode && !Text.Contains("\n"))
            return "/* " + Text + " */";

        var cw = new CodeWriter();

        using (BlockMode ? cw.Block("\r/*\n", "\r */\n", " * ") : cw.Indent("// "))
        {
            cw.WriteLine(Text);
        }

        return cw.ToString();
    }

    public static implicit operator Comment(string text)
    {
        return new Comment(text);
    }
}
