using System.Collections.Immutable;
using System.Linq;

namespace Fujiberg.Coder.CSharp;

public sealed record CodeFile
{
    public IImmutableSet<Declaration> Declarations { get; init; } = [];
    public bool EnableNullability { get; init; } = true;
    public Comment? HeaderComment { get; init; }

    public string ToCode(CodeGenContext? context = null)
    {
        context ??= new CodeGenContext();

        var cw = new CodeWriter();

        if (HeaderComment is not null)
            cw.WriteLine(HeaderComment.ToCode());

        if (EnableNullability)
            cw.WriteLine("#nullable enable\n");

        if (context.NamespaceReferences.Any())
        {
            foreach (var nsRef in context.NamespaceReferences.OrderBy(x => x))
                cw.WriteLine($"using {nsRef.ToCode(context with {IncludeGlobal = false}, false)};");
            cw.WriteLine();
        }

        var namespaces = Declarations.OrderBy(x => x.Namespace.ToCode(context with {IncludeGlobal = true}, false))
            .GroupBy(x => x.Namespace)
            .ToArray();

        foreach (var ns in namespaces)
        {
            cw.WriteLine($"namespace {ns.Key.ToCode(context with {IncludeGlobal = false}, false)}");
            using (cw.Block())
            {
                foreach (var decl in ns)
                    cw.WriteLine(decl.ToCode(context));
            }
        }

        return cw.ToString();
    }

    public CodeFile WithDeclaration(Declaration type)
    {
        return this with {Declarations = Declarations.Add(type)};
    }
}

public sealed record CodeGenContext
{
    public bool IncludeGlobal { get; init; } = true;
    public IImmutableSet<Namespace> NamespaceReferences { get; init; } = [];
}
