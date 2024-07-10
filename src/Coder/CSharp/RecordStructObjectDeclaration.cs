namespace Fujiberg.Coder.CSharp;

public sealed record RecordStructObjectDeclaration : ValueObjectDeclaration
{
    public RecordStructObjectDeclaration(string name) : base("record struct", name)
    {
    }
}
