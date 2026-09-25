using System;
using System.IO;
using System.Threading.Tasks;
using VerifyXunit;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    /// <summary>
    /// Locations used by the golden-file tests. The fixtures themselves are managed by Verify
    /// (<c>Golden/Fixtures/&lt;name&gt;.verified.yml</c>).
    /// </summary>
    internal static class GoldenFile
    {
        /// <summary>
        /// Verifies <paramref name="yaml"/> against the fixture <c>Golden/Fixtures/&lt;name&gt;.verified.yml</c>.
        /// Verify's scrubbers are disabled because the YAML must match verbatim: they would otherwise rewrite
        /// values such as <c>/tmp</c> (the temp path on Linux), GUIDs and the machine name.
        /// </summary>
        /// <param name="name">The fixture name without its extension.</param>
        /// <param name="yaml">The YAML to verify.</param>
        /// <returns>A task that completes when the comparison has finished.</returns>
        internal static Task Verify(string name, string yaml)
        {
            return Verifier.Verify(yaml, "yml")
                .DisableScrubbers()
                .UseDirectory(Path.Combine(SourceRoot(), "test", "DockerComposeFluent.Tests.Unit", "Golden", "Fixtures"))
                .UseFileName(name);
        }

        /// <summary>
        /// The folder the fixtures live in, in the source tree.
        /// </summary>
        internal static string FixturesDirectory
        {
            get { return Path.Combine(SourceRoot(), "test", "DockerComposeFluent.Tests.Unit", "Golden", "Fixtures"); }
        }

        /// <summary>
        /// Normalises line endings and the trailing newline so text compares the same on every platform.
        /// </summary>
        /// <param name="yaml">The text to normalise.</param>
        /// <returns>The normalised text.</returns>
        internal static string Normalise(string yaml)
        {
            return yaml.Replace("\r\n", "\n").TrimEnd('\n');
        }

        /// <summary>
        /// Finds the repository root by looking upwards from the test assembly for the solution file.
        /// </summary>
        /// <returns>The repository root folder.</returns>
        internal static string SourceRoot()
        {
            DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !File.Exists(Path.Combine(directory.FullName, "DockerComposeFluent.slnx")))
            {
                directory = directory.Parent;
            }

            if (directory == null)
            {
                throw new InvalidOperationException("Could not find the repository root (DockerComposeFluent.slnx).");
            }

            return directory.FullName;
        }
    }
}
