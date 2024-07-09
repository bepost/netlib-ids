using System.Collections.Immutable;

namespace Fujiberg.Coder.CSharp;

public sealed record NamespaceDeclaration
{
    public required Namespace Name { get; init; }
    public IImmutableSet<NamespaceDeclaration> NamespaceDeclarations { get; init; } = [];
    public IImmutableSet<TypeDeclaration> TypeDeclarations { get; init; } = [];

    public NamespaceDeclaration AddNamespaces(params NamespaceDeclaration[] namespaces)
    {
        return this with {NamespaceDeclarations = [..NamespaceDeclarations, ..namespaces]};
    }

    public NamespaceDeclaration AddTypes(params TypeDeclaration[] type)
    {
        return this with {TypeDeclarations = [..TypeDeclarations, ..type]};
    }

    public static NamespaceDeclaration Create(Namespace name)
    {
        return new NamespaceDeclaration {Name = name};
    }

    public string ToCode()
    {
        var cw = new CodeWriter();
        cw.WriteLine($"namespace {Name.ToCode(useGlobalPrefix: false)}");
        using (cw.Block())
        {
            foreach (var ns in NamespaceDeclarations)
                cw.WriteLine(ns.ToCode());
            foreach (var type in TypeDeclarations)
                cw.WriteLine(type.ToCode());
        }

        return cw.ToString();
    }
}
