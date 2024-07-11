using System.Collections.Immutable;

namespace Fujiberg.Coder.CSharp;

public abstract record ObjectDeclaration : TypeDeclaration
{
    private protected ObjectDeclaration(string name) : base(name)
    {
    }

    public IImmutableSet<Constructor> Constructors { get; set; }
    public IImmutableSet<Field> Fields { get; set; }
    public IImmutableSet<QualifiedName> Interfaces { get; init; } = [];
    public IImmutableSet<Method> Methods { get; set; }
    public bool Partial { get; init; }
    public IImmutableSet<Property> Properties { get; set; }
}
