using System.Collections.Generic;
using System.IO;
using System.Linq;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;
using Xunit.Sdk;

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
        public void Scenario_MatchesItsFixture(string name)
        {
            GoldenFile.AssertMatches(name, GoldenScenarios.All[name]());
        }

        [Fact]
        public void EveryFixtureHasAScenario_AndEveryScenarioHasAFixture()
        {
            HashSet<string> fixtures = new HashSet<string>(
                Directory.GetFiles(GoldenFile.FixturesDirectory, "*.yml").Select(path => Path.GetFileNameWithoutExtension(path)!));
            HashSet<string> scenarios = new HashSet<string>(GoldenScenarios.All.Keys);

            Assert.Empty(fixtures.Except(scenarios));
            Assert.Empty(scenarios.Except(fixtures));
        }

        [Fact]
        public void AssertMatches_MissingFixture_FailsWithHowToCreateIt()
        {
            XunitException exception = Assert.Throws<XunitException>(() => GoldenFile.AssertMatches("no-such-fixture", new DockerComposeFile()));

            Assert.Contains("UPDATE_GOLDEN=1", exception.Message);
            Assert.Contains("no-such-fixture.yml", exception.Message);
        }

        [Fact]
        public void AssertMatches_DifferentOutput_ShowsExpectedAndActual()
        {
            DockerComposeFile different = new DockerComposeBuilder().WithName("not-the-fixture").Build();

            XunitException exception = Assert.Throws<XunitException>(() => GoldenFile.AssertMatches("basic", different));

            Assert.Contains("--- expected ---", exception.Message);
            Assert.Contains("--- actual ---", exception.Message);
            Assert.Contains("not-the-fixture", exception.Message);
        }

        [Fact]
        public void Normalise_IgnoresLineEndingsAndTrailingNewlines()
        {
            Assert.Equal("a\nb", GoldenFile.Normalise("a\r\nb\r\n\r\n"));
        }
    }
}
