using System.Runtime.CompilerServices;
using VerifyTests;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    /// <summary>
    /// Global Verify settings, applied once when the test assembly loads.
    /// </summary>
    internal static class VerifySetup
    {
        /// <summary>
        /// Writes fixtures without a byte-order mark, so they are plain UTF-8 files that Compose reads as they are.
        /// </summary>
        [ModuleInitializer]
        internal static void Initialise()
        {
            VerifierSettings.UseUtf8NoBom();
        }
    }
}
