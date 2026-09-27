using DockerComposeFluent.Builders;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>logging</c> golden scenarios.
    /// </summary>
    internal static class LoggingScenarios
    {
        /// <summary>
        /// A driver with no options, a driver with options set individually and in bulk, and an existing
        /// definition passed straight in.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("plain", service => service.WithImage("nginx").WithLogging("json-file"))
                .WithService("syslog", service => service
                    .WithImage("nginx")
                    .WithLogging(logging => logging
                        .WithDriver("syslog")
                        .WithOption("syslog-address", "tcp://192.168.0.42:123")))
                .WithService("bulk", service => service
                    .WithImage("nginx")
                    .WithLogging(logging => logging
                        .WithDriver("json-file")
                        .WithOptions(new Dictionary<string, string> { ["max-size"] = "10m", ["max-file"] = "3" })))
                .WithService("existing", service => service.WithImage("nginx").WithLogging(new LoggingDefinition { Driver = "none" }))
                .Build();
        }
    }
}
