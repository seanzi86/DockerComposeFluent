using System;
using System.IO;
using DockerComposeFluent.Models;
using Xunit.Sdk;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    /// <summary>
    /// Compares generated compose YAML with the fixture files checked in under <c>Golden/Fixtures</c>.
    /// Run the tests with <c>UPDATE_GOLDEN=1</c> to create or refresh fixtures from the current output.
    /// </summary>
    internal static class GoldenFile
    {
        private const string _updateVariable = "UPDATE_GOLDEN";

        /// <summary>
        /// The folder the fixtures are copied to next to the test assembly.
        /// </summary>
        internal static string FixturesDirectory
        {
            get { return Path.Combine(AppContext.BaseDirectory, "Golden", "Fixtures"); }
        }

        /// <summary>
        /// Asserts that the YAML for <paramref name="file"/> matches the fixture with the given name.
        /// </summary>
        /// <param name="fixtureName">The fixture file name without its <c>.yml</c> extension.</param>
        /// <param name="file">The compose file to serialise.</param>
        internal static void AssertMatches(string fixtureName, DockerComposeFile file)
        {
            string actual = Normalise(file.ToYaml());

            if (Environment.GetEnvironmentVariable(_updateVariable) == "1")
            {
                Update(fixtureName, actual);
                return;
            }

            string path = Path.Combine(FixturesDirectory, fixtureName + ".yml");
            if (!File.Exists(path))
            {
                throw new XunitException(
                    $"The fixture 'Golden/Fixtures/{fixtureName}.yml' does not exist. "
                    + $"Run the tests with {_updateVariable}=1 to create it from the current output.");
            }

            string expected = Normalise(File.ReadAllText(path));
            if (expected != actual)
            {
                throw new XunitException(
                    $"The generated YAML does not match 'Golden/Fixtures/{fixtureName}.yml'. "
                    + $"If the change is intended, run the tests with {_updateVariable}=1 and review the diff.\n"
                    + $"--- expected ---\n{expected}\n--- actual ---\n{actual}");
            }
        }

        /// <summary>
        /// Normalises line endings and the trailing newline so fixtures compare the same on every platform.
        /// </summary>
        /// <param name="yaml">The text to normalise.</param>
        /// <returns>The normalised text.</returns>
        internal static string Normalise(string yaml)
        {
            return yaml.Replace("\r\n", "\n").TrimEnd('\n');
        }

        private static void Update(string fixtureName, string actual)
        {
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                throw new InvalidOperationException($"{_updateVariable} must not be used on CI.");
            }

            string path = Path.Combine(SourceRoot(), "test", "DockerComposeFluent.Tests.Unit", "Golden", "Fixtures", fixtureName + ".yml");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, actual + "\n");
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
                throw new InvalidOperationException("Could not find the repository root (DockerComposeFluent.slnx) to update fixtures.");
            }

            return directory.FullName;
        }
    }
}
