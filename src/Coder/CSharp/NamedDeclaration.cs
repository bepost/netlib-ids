namespace Fujiberg.Coder.CSharp;

public abstract record NamedDeclaration
{
    private protected NamedDeclaration(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
    public abstract string ToCode();
}
