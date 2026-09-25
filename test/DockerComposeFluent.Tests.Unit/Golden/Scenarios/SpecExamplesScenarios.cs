using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>spec-examples</c> golden scenarios: examples modelled on the ones in the
    /// <see href="https://github.com/compose-spec/compose-spec">compose-spec</see>, rebuilt with the fluent API.
    /// </summary>
    internal static class SpecExamplesScenarios
    {
        /// <summary>
        /// The network aliases example from the spec: one service reachable under different aliases on each network.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#aliases"/>
        /// </summary>
        [Scenario]
        internal static DockerComposeFile NetworkAliases()
        {
            return new DockerComposeBuilder()
                .WithService("frontend", service => service
                    .WithImage("example/webapp")
                    .WithNetwork("front-tier")
                    .WithNetwork("back-tier"))
                .WithService("monitoring", service => service
                    .WithImage("example/monitoring")
                    .WithNetwork("admin"))
                .WithService("backend", service => service
                    .WithImage("example/backend")
                    .WithNetwork("back-tier", network => network.WithAlias("database"))
                    .WithNetwork("admin", network => network.WithAlias("backend-admin")))
                .WithNetwork("front-tier", network => network.WithDriver("bridge"))
                .WithNetwork("back-tier", network => network.WithDriver("bridge"))
                .WithNetwork("admin", network => network.WithDriver("bridge"))
                .Build();
        }

        /// <summary>
        /// The short volume syntax examples from the spec: a host path, a relative path, a home-relative
        /// read-only path and a named volume.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#short-syntax-5"/>
        /// </summary>
        [Scenario]
        internal static DockerComposeFile ShortVolumes()
        {
            return new DockerComposeBuilder()
                .WithService("backend", service => service
                    .WithImage("example/backend")
                    .WithVolumes(new[]
                    {
                        "/var/run/postgres/postgres.sock:/var/run/postgres/postgres.sock",
                        "dbdata:/var/lib/postgresql/data",
                        "./cache:/tmp/cache",
                        "~/configs:/etc/configs/:ro"
                    }))
                .WithVolume("dbdata", volume => volume.WithDriver("local"))
                .Build();
        }
    }
}
