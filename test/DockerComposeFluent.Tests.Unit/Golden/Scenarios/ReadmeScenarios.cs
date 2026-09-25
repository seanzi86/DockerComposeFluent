using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>readme</c> golden scenarios.
    /// </summary>
    internal static class ReadmeScenarios
    {
        [Scenario]
        internal static DockerComposeFile QuickStart()
        {
            // README:begin
            DockerComposeFile file = new DockerComposeBuilder()
                .WithName("shop")
                .WithService("web", service => service
                    .WithImage("nginx:1.27")
                    .WithPort(8080, 80)
                    .WithVolume("./site", "/usr/share/nginx/html", readOnly: true)
                    .WithNetwork("front"))
                .WithService("db", service => service
                    .WithImage("postgres:17")
                    .WithEnvironment("POSTGRES_DB", "shop")
                    .WithEnvironment("POSTGRES_PASSWORD")
                    .WithVolume("db-data", "/var/lib/postgresql/data")
                    .WithNetwork("back"))
                .WithNetwork("front", network => network.WithDriver("bridge"))
                .WithNetwork("back", network => network.WithInternal(true))
                .WithVolume("db-data", volume => volume.WithDriver("local"))
                .Build();
            // README:end

            return file;
        }
    }
}
