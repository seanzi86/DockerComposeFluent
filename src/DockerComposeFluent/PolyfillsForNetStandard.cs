#if NETSTANDARD2_0
// The C# compiler requires this exact type to exist in order to allow `init`
// accessors to compile. It ships in the .NET 5+ BCL, which netstandard2.0
// consumers may not have, so we declare an empty marker type of our own here.
// It has no effect at runtime.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
#endif
