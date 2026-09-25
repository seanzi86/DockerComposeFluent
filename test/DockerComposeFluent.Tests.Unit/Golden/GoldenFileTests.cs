using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    public class GoldenFileTests
    {
        public static TheoryData<string> ScenarioNames
        {
            get
            {
                TheoryData<string> names = new TheoryData<string>();
                foreach (string name in GoldenScenarios.All.Keys)
                {
                    names.Add(name);
                }

                return names;
            }
        }

        [Theory]
        [MemberData(nameof(ScenarioNames))]
        public Task Scenario_MatchesItsFixture(string name)
        {
            return GoldenFile.Verify(name, GoldenScenarios.All[name]().ToYaml());
        }

        [Fact]
        public void EveryFixtureHasAScenario_AndEveryScenarioHasAFixture()
        {
            HashSet<string> fixtures = new HashSet<string>(
                Directory.GetFiles(GoldenFile.FixturesDirectory, "*.verified.yml")
                    .Select(path => Path.GetFileName(path).Replace(".verified.yml", string.Empty)));
            HashSet<string> scenarios = new HashSet<string>(GoldenScenarios.All.Keys);

            Assert.Empty(fixtures.Except(scenarios));
            Assert.Empty(scenarios.Except(fixtures));
        }

        [Fact]
        public void Normalise_IgnoresLineEndingsAndTrailingNewlines()
        {
            Assert.Equal("a\nb", GoldenFile.Normalise("a\r\nb\r\n\r\n"));
        }
    }
}
