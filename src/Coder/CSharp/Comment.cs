namespace Fujiberg.Coder.CSharp;

#pragma warning disable CA1847

public sealed record Comment
{
    public Comment(string text)
    {
        Text = text;
    }

    public bool BlockMode { get; init; }
    public string Text { get; init; }

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
}
