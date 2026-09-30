using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>include</c> golden scenarios.
    /// </summary>
    internal static class IncludeScenarios
    {
        /// <summary>
        /// A short-form include (a bare path), a long-form include with an env file and project directory,
        /// and the raw-definition and builder overloads.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithInclude("./common.yml")
                .WithInclude(include => include
                    .WithPath("./other.yml")
                    .WithEnvFile("./other.env")
                    .WithProjectDirectory("."))
                .WithInclude(new IncludeDefinition { Path = new[] { "./common.yml" } })
                .WithService("web", service => service.WithImage("example/web"))
                .Build();
        }
    }
}
