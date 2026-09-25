using System;
using System.IO;
using System.Linq;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    public class ReadmeTests
    {
        [Fact]
        public void QuickStartCode_IsTheCodeOfTheReadmeScenario()
        {
            string snippet = ReadmeSnippet();

            Assert.True(
                ReadmeText().Contains(snippet),
                "README.md does not contain the quick-start code from the 'readme/quick-start' scenario in ReadmeScenarios.cs. "
                + "Update the README so the two match:\n" + snippet);
        }

        [Fact]
        public void QuickStartYaml_IsTheReadmeFixture()
        {
            string yaml = GoldenFile.Normalise(File.ReadAllText(Path.Combine(GoldenFile.FixturesDirectory, "readme", "quick-start.verified.yml")));

            Assert.True(
                ReadmeText().Contains(yaml),
                "README.md does not contain the YAML in Golden/Fixtures/readme/quick-start.verified.yml. Update the README so the two match:\n" + yaml);
        }

        private static string ReadmeText()
        {
            return File.ReadAllText(Path.Combine(GoldenFile.SourceRoot(), "README.md")).Replace("\r\n", "\n");
        }

        private static string ReadmeSnippet()
        {
            string[] lines = File.ReadAllText(Path.Combine(GoldenFile.SourceRoot(), "test", "DockerComposeFluent.Tests.Unit", "Golden", "Scenarios", "ReadmeScenarios.cs"))
                .Replace("\r\n", "\n")
                .Split('\n');

            int begin = Array.FindIndex(lines, line => line.Contains("// README:begin"));
            int end = Array.FindIndex(lines, line => line.Contains("// README:end"));
            if (begin < 0 || end < begin)
            {
                throw new InvalidOperationException("The README:begin and README:end markers are missing from ReadmeScenarios.cs.");
            }

            string[] code = lines.Skip(begin + 1).Take(end - begin - 1).ToArray();
            int indent = lines[begin].Length - lines[begin].TrimStart().Length;

            return string.Join("\n", code.Select(line => line.Length >= indent ? line.Substring(indent) : line));
        }
    }
}
