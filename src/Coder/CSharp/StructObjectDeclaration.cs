namespace Fujiberg.Coder.CSharp;

public sealed record StructObjectDeclaration : ValueObjectDeclaration
{
    public StructObjectDeclaration(string name) : base("struct", name)
    {
    }
}
