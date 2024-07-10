namespace Fujiberg.Coder.CSharp;

public sealed record ClassDeclaration : ReferenceObjectDeclaration
{
    public ClassDeclaration(string name) : base("class", name)
    {
    }
}
