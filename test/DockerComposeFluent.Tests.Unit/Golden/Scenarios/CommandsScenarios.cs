using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>commands</c> golden scenarios.
    /// </summary>
    internal static class CommandsScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("node:22")
                    .WithCommand(new[] { "node", "server.js", "--port", "8080" })
                    .WithEntrypoint(Array.Empty<string>()))
                .WithService("shell", service => service
                    .WithImage("node:22")
                    .WithCommand("npm start")
                    .WithEntrypoint("/docker-entrypoint.sh"))
                .Build();
        }
    }
}
