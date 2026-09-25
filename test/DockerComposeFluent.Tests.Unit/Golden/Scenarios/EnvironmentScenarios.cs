using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>environment</c> golden scenarios.
    /// </summary>
    internal static class EnvironmentScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("map", service => service
                    .WithImage("nginx")
                    .WithEnvironment("RACK_ENV", "development")
                    .WithEnvironment("SHOW", true)
                    .WithEnvironment("PORT", 8080)
                    .WithEnvironment("EMPTY", "")
                    .WithEnvironment("USER_INPUT"))
                .WithService("list", service => service
                    .WithImage("nginx")
                    .WithEnvironment(new[] { "RACK_ENV=development", "SHOW=true", "URL=http://x?a=b", "EMPTY=", "USER_INPUT" }))
                .WithService("mixed", service => service
                    .WithImage("nginx")
                    .WithEnvironment("A", "1")
                    .WithEnvironment("B")
                    .WithEnvironment(new[] { "C=3" }))
                .Build();
        }
    }
}
