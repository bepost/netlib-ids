using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public abstract record Member
{
    private protected Member(QualifiedName type, string name)
    public bool Abstract { get; init; }
    public bool New { get; init; }
    public bool Override { get; init; }
    public bool Partial { get; init; }
    public bool Virtual { get; init; }
    protected abstract string AdditionalModifiers { get; }
    protected abstract string ImplementationToCode();
}

public sealed record Field
{
    public Field(QualifiedName type, string name)
    {
        Name = name;
        Type = type;
    }

    public Accessor Accessor { get; init; } = Accessor.Private;
    public IImmutableSet<Attribution> Attributes { get; set; } = [];
    public Expression? Initializer { get; init; }
    public string Name { get; init; }
    public bool ReadOnly { get; init; }
    public bool Static { get; init; }
    public QualifiedName Type { get; set; }

    public string ToCode()
    {
        var cw = new CodeWriter();

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(Accessor.ToCode());

        if (Static)
            cw.Write("static ");
        if (ReadOnly)
            cw.Write("readonly ");

        cw.Write(Type.ToCode());
        cw.Write(" ");
        cw.Write(Name);

        if (Initializer is not null)
        {
            cw.Write(" = ");
            cw.Write(Initializer.ToCode());
        }

        cw.WriteLine(";");

        return cw.ToString();
    }
}

public sealed record Method : Member
{
    public Method(QualifiedName returnType, string name) : base(returnType, name)
    {
    }

    public string? Body { get; init; }
    public IImmutableList<Parameter> Parameters { get; init; } = [];
    protected override string AdditionalModifiers => "";

    protected override string ImplementationToCode()
    {
        var cw = new CodeWriter();

        cw.Write("(");
        cw.Write(string.Join(", ", Parameters.Select(x => x.ToCode())));
        cw.Write(")");

        if (Abstract && Body is null)
        {
            cw.WriteLine(";");
            return cw.ToString();
        }

        cw.WriteLine();
        using (cw.Block())
        {
            if (Body is not null)
                cw.Write(Body);
        }

        return cw.ToString();
    }
}

public sealed record Constructor
{
    public bool Abstract { get; init; }
    public Accessor Accessor { get; init; } = Accessor.Private;
    public IImmutableSet<Attribution> Attributes { get; set; } = [];

    // TODO Improve
    public bool New { get; init; }
    public bool Override { get; init; }
    public bool Static { get; init; }
    public bool Virtual { get; init; }
    // TODO Improve

    public string ToCode(string containingTypeName)
    {
        var cw = new CodeWriter();

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(Accessor.ToCode());

        if (Abstract)
            cw.Write("abstract ");
        if (Virtual)
            cw.Write("virtual ");
        if (Override)
            cw.Write("override ");
        if (New)
            cw.Write("new ");
        if (Static)
            cw.Write("static ");

        cw.Write(containingTypeName);

        using (cw.Block())
        {
            // TODO Getter/Setter
            // TODO Autoproperties
        }

        return cw.ToString();
    }
}

public sealed record Property
{
    public Property(QualifiedName type, string name)
    {
        Name = name;
        Type = type;
    }

    public Accessor Accessor { get; init; } = Accessor.Private;
    public IImmutableSet<Attribution> Attributes { get; set; } = [];

    // TODO Improve
    public IImmutableList<Expression>? BaseCall { get; init; }
    public string Body { get; init; } = "";
    public string Name { get; init; }
    public IImmutableList<Parameter> Parameters { get; init; } = [];
    public bool Static { get; init; }

    // TODO Improve
    public IImmutableList<Expression>? ThisCall { get; init; }
    public QualifiedName Type { get; init; }

    public string ToCode(string containingTypeName)
    {
        var cw = new CodeWriter();

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(Accessor.ToCode());

        if (Static)
            cw.Write("static ");

        cw.Write(containingTypeName);
        cw.Write("(");
        cw.Write(string.Join(", ", Parameters.Select(x => x.ToCode())));
        cw.WriteLine(")");

        if (ThisCall is not null)
        {
            using (cw.Indent())
            {
                cw.Write(": base(");
                cw.Write(string.Join(", ", ThisCall.Select(x => x.ToCode())));
                cw.WriteLine(")");
            }
        }

        if (BaseCall is not null)
        {
            using (cw.Indent())
            {
                cw.Write(": base(");
                cw.Write(string.Join(", ", BaseCall.Select(x => x.ToCode())));
                cw.WriteLine(")");
            }
        }

        using (cw.Block())
        {
            cw.Write(Body);
        }

        return cw.ToString();
    }
}
