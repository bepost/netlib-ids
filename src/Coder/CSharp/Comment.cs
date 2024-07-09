using System.Collections.Generic;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

#pragma warning disable CA1847

public sealed record Comment
{
    public bool BlockMode { get; init; }
    public required string Text { get; init; }

    public static Comment CreateBlock(string text)
    {
        return new Comment {Text = text, BlockMode = true};
    }

    public static Comment CreateLine(string text)
    {
        return new Comment {Text = text};
    }

    public static IEnumerable<Comment> CreateLines(params string[] text)
    {
        return text.Select(CreateLine);
    }

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
