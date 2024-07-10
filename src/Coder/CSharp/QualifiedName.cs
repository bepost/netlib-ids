using System;
using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record QualifiedName
{
    public QualifiedName(Namespace @namespace, string name)
    {
        Namespace = @namespace;
        Name = name;
    }

    public IImmutableList<QualifiedName> Generics { get; init; } = [];
    public string Name { get; init; }
    public Namespace Namespace { get; init; }

    public string ToCode(bool useGlobalPrefix = true)
    {
        var type = Namespace.ToCode(includeTrailingDot:true, useGlobalPrefix:useGlobalPrefix) + Name;
        if (Generics.Any())
            type = type + "<" + string.Join(", ", Generics.Select(x => x.ToCode())) + ">";
        return type;
    }

    public static implicit operator QualifiedName(string fqdn)
    {
        var ix = fqdn.LastIndexOf(value:'.');
        if (ix == -1)
            return new QualifiedName(Namespace.Global, fqdn);

        return new QualifiedName(fqdn[..ix], fqdn[(ix + 1)..]);
    }

    public static implicit operator QualifiedName(Type type)
    {
        // TODO Add generics
        return new QualifiedName(type.Namespace ?? Namespace.Global, type.Name);
    }
}
