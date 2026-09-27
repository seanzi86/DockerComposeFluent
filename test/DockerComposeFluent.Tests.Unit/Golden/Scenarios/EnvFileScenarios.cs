using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>env_file</c> golden scenarios.
    /// </summary>
    internal static class EnvFileScenarios
    {
        /// <summary>
        /// A single path, several plain paths, the common required overload, and a fully configured entry, mixed
        /// with plain paths in the same list since Compose allows each entry its own shape.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("single", service => service.WithImage("nginx").WithEnvFile("./.env"))
                .WithService("list", service => service.WithImage("nginx").WithEnvFiles(new[] { "./a.env", "./b.env" }))
                .WithService("optional", service => service
                    .WithImage("nginx")
                    .WithEnvFile("./default.env")
                    .WithEnvFile("./override.env", required: false))
                .WithService("mixed", service => service
                    .WithImage("nginx")
                    .WithEnvFile("./.env")
                    .WithEnvFile(entry => entry.WithPath("./raw.env").WithFormat(EnvFileFormat.Raw).WithRequired(true)))
                .Build();
        }
    }
}
