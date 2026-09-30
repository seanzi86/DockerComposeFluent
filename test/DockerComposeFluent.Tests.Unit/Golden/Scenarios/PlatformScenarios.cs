using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>platform</c> golden scenarios.
    /// </summary>
    internal static class PlatformScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("amd64", service => service.WithImage("example/app").WithPlatform("linux/amd64"))
                .WithService("arm64", service => service.WithImage("example/app").WithPlatform("linux/arm64"))
                .Build();
        }
    }
}
