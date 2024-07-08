namespace Fujiberg.Coder.CSharp;

public sealed record QualifiedName
{
    public required string Name { get; init; }
    public required Namespace Namespace { get; init; }

    public string ToCode(CodeGenContext context)
    {
        if (context.NamespaceReferences.Contains(Namespace))
            return Name;

        return Namespace.ToCode(context, true)+Name;

    }

    public static implicit operator QualifiedName(string fqdn)
    {
        if (fqdn.Contains("."))
        {
            return new QualifiedName
            {
                Namespace = new Namespace(fqdn[..fqdn.LastIndexOf('.')]), Name = fqdn[(fqdn.LastIndexOf('.') + 1)..]
            };
        }

        return new QualifiedName {Namespace = Namespace.Global, Name = fqdn};
    }
}
