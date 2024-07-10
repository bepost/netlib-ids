using System.Linq;

namespace Fujiberg.Coder.CSharp;

public abstract record ValueObjectDeclaration : ObjectDeclaration
{
    private readonly string _kind;

    private protected ValueObjectDeclaration(string kind, string name) : base(name)
    {
        _kind = kind;
    }

    public override string ToCode()
    {
        var cw = new CodeWriter();

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(Accessor.ToCode());

        if (IsPartial)
            cw.Write("partial ");

        cw.Write($"{_kind} ");

        cw.Write(Name);

        if (Interfaces.Any())
        {
            cw.Write(" : ");
            cw.Write(string.Join(", ", Interfaces.Select(x => x.ToCode())));
        }
        // TODO Generic Constraints

        using (cw.Block())
        {
            // TODO Members
        }

        return cw.ToString();
    }
}
