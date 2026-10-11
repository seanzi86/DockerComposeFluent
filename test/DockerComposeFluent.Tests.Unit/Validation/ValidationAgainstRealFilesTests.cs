using System.Linq;
using DockerComposeFluent.Models;
using DockerComposeFluent.Tests.Unit.Golden;
using DockerComposeFluent.Validation;

namespace DockerComposeFluent.Tests.Unit.Validation
{
    public class ValidationAgainstRealFilesTests
    {
        // Every golden fixture is accepted by a real "docker compose config" in CI, so Compose rejects none of
        // them. An error here would therefore be a false positive in the validator.
        [Theory]
        [MemberData(nameof(ScenarioNames))]
        public void GoldenScenario_HasNoErrors(string name)
        {
            DockerComposeFile file = GoldenScenarios.All[name]();

            ValidationResult result = file.Validate();

            Assert.True(result.IsValid, string.Join("\n", result.Errors.Select(error => error.ToString())));
        }

        public static TheoryData<string> ScenarioNames()
        {
            TheoryData<string> names = new TheoryData<string>();
            foreach (string name in GoldenScenarios.All.Keys)
            {
                names.Add(name);
            }

            return names;
        }
    }
}
