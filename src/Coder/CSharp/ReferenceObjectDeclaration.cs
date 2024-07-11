using System.Linq;

namespace Fujiberg.Coder.CSharp;

public abstract record ReferenceObjectDeclaration : ObjectDeclaration
{
    private readonly string _kind;

    private protected ReferenceObjectDeclaration(string kind, string name) : base(name)
    {
        _kind = kind;
    }

    public bool Abstract { get; init; }
    public QualifiedName? Parent { get; init; }
    public bool Sealed { get; init; }
    public bool Static { get; init; }

    public override string ToCode()
    {
        var cw = new CodeWriter();

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(Accessor.ToCode());

        if (Sealed)
            cw.Write("sealed ");
        if (Abstract)
            cw.Write("abstract ");
        if (Static)
            cw.Write("static ");
        if (Partial)
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
            foreach (var field in Fields)
                cw.WriteLine(field.ToCode());
            foreach (var ctor in Constructors)
                cw.WriteLine(ctor.ToCode(Name));
            foreach (var prop in Properties)
                cw.WriteLine(prop.ToCode());
            foreach (var method in Methods)
                cw.WriteLine(method.ToCode());
        }

        return cw.ToString();
    }
}
