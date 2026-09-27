using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>profiles</c> golden scenarios.
    /// </summary>
    internal static class ProfilesScenarios
    {
        /// <summary>
        /// A single profile, several profiles added one at a time and in bulk, and a service with none.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("frontend", service => service.WithImage("frontend").WithProfile("frontend"))
                .WithService("phpmyadmin", service => service
                    .WithImage("phpmyadmin")
                    .WithDependsOn("db")
                    .WithProfile("debug"))
                .WithService("multiple", service => service.WithImage("nginx").WithProfiles(new[] { "frontend", "debug" }))
                .WithService("db", service => service.WithImage("postgres"))
                .WithService("always", service => service.WithImage("nginx"))
                .Build();
        }
    }
}
