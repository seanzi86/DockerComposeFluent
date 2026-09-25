using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>basic</c> golden scenarios.
    /// </summary>
    internal static class BasicScenarios
    {
        [Scenario]
        internal static DockerComposeFile Demo()
        {
            return new DockerComposeBuilder()
                .WithName("demo")
                .WithService("web", service => service.WithContainerName("web-container").WithImage("nginx:latest"))
                .WithNetwork("frontend", network => network.WithDriver("bridge"))
                .WithVolume("data", volume => volume.WithDriver("local").WithName("demo_data"))
                .Build();
        }
    }
}
