using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record TypeDeclaration
{
    public Accessor Accessor { get; init; } = Accessor.Internal;
    public IImmutableSet<Attribution> Attributes { get; init; } = [];
    public bool IsAbstract { get; init; }
    public bool IsPartial { get; init; }
    public bool IsSealed { get; init; }
    public bool IsStatic { get; init; }
    public TypeKind Kind { get; init; }
    public required string Name { get; init; }
    public QualifiedName? Parent { get; init; }

    public TypeDeclaration AddAttributes(params Attribution[] attribute)
    {
        return this with {Attributes = [..Attributes, ..attribute]};
    }

    public TypeDeclaration AsAbstract()
    {
        return this with {IsAbstract = true};
    }

    public TypeDeclaration AsPartial()
    {
        return this with {IsPartial = true};
    }

    public TypeDeclaration AsPublic()
    {
        return this with {Accessor = Accessor.Public};
    }

    public TypeDeclaration AsSealed()
    {
        return this with {IsSealed = true};
    }

    public TypeDeclaration AsStatic()
    {
        return this with {IsStatic = true};
    }

    public static TypeDeclaration Create(string name, TypeKind kind)
    {
        return new TypeDeclaration {Name = name, Kind = kind};
    }

    public static TypeDeclaration CreateClass(string name)
    {
        return Create(name, TypeKind.Class);
    }

    public static TypeDeclaration CreateInterface(string name)
    {
        return Create(name, TypeKind.Interface);
    }

    public static TypeDeclaration CreateRecord(string name)
    {
        return Create(name, TypeKind.Record);
    }

    public static TypeDeclaration CreateRecordStruct(string name)
    {
        return Create(name, TypeKind.RecordStruct);
    }

    public static TypeDeclaration CreateStruct(string name)
    {
        return Create(name, TypeKind.Struct);
    }

    public string ToCode()
    {
        var cw = new CodeWriter();
        // TODO XmlDocs

        var attrs = Attributes.Select(x => x.ToCode())
            .OrderBy(x => x);
        foreach (var attr in attrs)
            cw.WriteLine(attr);

        cw.Write(
            Accessor switch
            {
                Accessor.Public => "public ",
                Accessor.Protected => "protected ",
                Accessor.Internal => "internal ",
                Accessor.ProtectedInternal => "protected internal ",
                Accessor.Private => "private ",
                _ => "unknown "
            }
        );

        if (IsAbstract)
            cw.Write("abstract ");
        if (IsSealed)
            cw.Write("sealed ");
        if (IsStatic)
            cw.Write("static ");
        if (IsAbstract)
            cw.Write("abstract ");
        if (IsPartial)
            cw.Write("partial ");

        cw.Write(
            Kind switch
            {
                TypeKind.Class => "class ",
                TypeKind.Struct => "struct ",
                TypeKind.Record => "record ",
                TypeKind.Interface => "interface ",
                TypeKind.RecordStruct => "record struct ",
                _ => "unknown "
            }
        );

        cw.Write(Name);

        // TODO Generics

        if (Parent is not null)
        {
            cw.Write(" : ");
            cw.Write(Parent.ToCode());

            // TODO interfaces and constraints
        }

        using (cw.Block())
        {
            // TODO members
        }

        return cw.ToString();
    }

    public TypeDeclaration WithParent(QualifiedName baseType)
    {
        return this with {Parent = baseType};
    }
}
