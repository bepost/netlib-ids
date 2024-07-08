using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record TypeDeclaration : Declaration
{
    public bool IsAbstract { get; init; }
    public bool IsPartial { get; init; }
    public bool IsSealed { get; init; }
    public bool IsStatic { get; init; }
    public TypeKind Kind { get; set; }
    public QualifiedName? Parent { get; init; }

    public static TypeDeclaration Class(QualifiedName name)
    {
        return new TypeDeclaration {Kind = TypeKind.Class, Namespace = name.Namespace, Name = name.Name};
    }

    public TypeDeclaration Public()
    {
        return this with {Accessor = Accessor.Public};
    }

    public TypeDeclaration Sealed()
    {
        return this with {IsSealed = true};
    }

    public TypeDeclaration Static()
    {
        return this with {IsStatic = true};
    }

    public override string ToCode(CodeGenContext context)
    {
        var cw = new CodeWriter();
        // TODO XmlDocs

        cw.WriteLines(
            Attributes.Select(x => x.ToCode(context))
                .OrderBy(x => x)
        );

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
            )
            .WriteIf(IsAbstract, "abstract ")
            .WriteIf(IsSealed, "sealed ")
            .WriteIf(IsStatic, "static ")
            .WriteIf(IsAbstract, "abstract ")
            .WriteIf(IsPartial, "partial ")
            .Write(
                Kind switch
                {
                    TypeKind.Class => "class ",
                    TypeKind.Struct => "struct ",
                    TypeKind.Record => "record ",
                    TypeKind.Interface => "interface ",
                    TypeKind.RecordStruct => "record struct ",
                    _ => "unknown "
                }
            )
            .Write(Name);

        // TODO Generics

        if (Parent is not null)
        {
            cw.Write(" : ")
                .Write(Parent.ToCode(context));

            // TODO interfaces and constraints
        }

        cw.WriteLine();
        using (cw.Block())
        {
            // TODO members
        }

        return cw.ToString();
    }

    public TypeDeclaration WithAttribute(Attribution attribute)
    {
        return this with {Attributes = Attributes.Add(attribute)};
    }

    public TypeDeclaration WithParent(QualifiedName baseType)
    {
        return this with {Parent = baseType};
    }
}

public enum TypeKind
{
    Class,
    Struct,
    Record,
    Interface,
    RecordStruct
}
