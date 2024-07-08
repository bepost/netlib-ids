// ReSharper disable CheckNamespace

#if NETSTANDARD2_0
namespace System.Runtime.CompilerServices
{
    public class RequiredMemberAttribute : Attribute;

    public class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string featureName)
        {
            FeatureName = featureName;
        }

        public string FeatureName { get; }
        public bool IsOptional { get; init; }
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    public class SetsRequiredMembersAttribute : Attribute;
}
#endif
