namespace Fujiberg.Coder.CSharp;

public static class AccessorExtensions
{
    public static string ToCode(this Accessor accessor)
    {
        return accessor switch
        {
            Accessor.Unset => "",
            Accessor.Public => "public ",
            Accessor.Protected => "protected ",
            Accessor.Internal => "internal ",
            Accessor.ProtectedInternal => "protected internal ",
            Accessor.Private => "private ",
            Accessor.PrivateProtected => "private protected ",
            _ => "unknown "
        };
    }
}
