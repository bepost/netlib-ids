namespace Fujiberg.Coder.CSharp;

public sealed record RecordObjectDeclaration : ReferenceObjectDeclaration
{
    public RecordObjectDeclaration(string name) : base("record", name)
    {
    }
}
