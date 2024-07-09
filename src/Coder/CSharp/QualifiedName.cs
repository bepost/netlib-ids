namespace Fujiberg.Coder.CSharp;

public sealed record QualifiedName
{
    public required string Name { get; init; }
    public required Namespace Namespace { get; init; }

    public static QualifiedName Create(string fqdn)
    {
        if (fqdn.Contains("."))
        {
            return new QualifiedName
            {
                Namespace = fqdn[..fqdn.LastIndexOf('.')], Name = fqdn[(fqdn.LastIndexOf('.') + 1)..]
            };
        }

        return new QualifiedName {Namespace = Namespace.Global, Name = fqdn};
    }

    public string ToCode()
    {
        return Namespace.ToCode(includeTrailingDot: true) + Name;
    }

    public static implicit operator QualifiedName(string fqdn)
    {
        return Create(fqdn);
    }
}
