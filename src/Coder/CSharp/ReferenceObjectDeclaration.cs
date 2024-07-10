using System.Linq;

namespace Fujiberg.Coder.CSharp;

public abstract record ReferenceObjectDeclaration : ObjectDeclaration
{
    private readonly string _kind;

    private protected ReferenceObjectDeclaration(string kind, string name) : base(name)
    {
        _kind = kind;
    }

    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsStatic { get; init; }
    public QualifiedName? Parent { get; init; }

    public override string ToCode()
    {
        var cw = new CodeWriter();

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(Accessor.ToCode());

        if (IsSealed)
            cw.Write("sealed ");
        if (IsAbstract)
            cw.Write("abstract ");
        if (IsStatic)
            cw.Write("static ");
        if (IsPartial)
            cw.Write("partial ");

        cw.Write($"{_kind} ");

        cw.Write(Name);

        var inheritance = Interfaces;
        if (Parent is not null)
            inheritance = [Parent, ..inheritance];

        if (inheritance.Any())
        {
            cw.Write(" : ");
            cw.Write(string.Join(", ", inheritance.Select(x => x.ToCode())));
        }

        // TODO Generic Constraints

        using (cw.Block())
        {
            // TODO Members
        }

        return cw.ToString();
    }
}
