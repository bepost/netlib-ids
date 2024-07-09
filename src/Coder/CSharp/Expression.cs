namespace Fujiberg.Coder.CSharp;

public abstract record Expression
{
    public abstract string ToCode();
}
