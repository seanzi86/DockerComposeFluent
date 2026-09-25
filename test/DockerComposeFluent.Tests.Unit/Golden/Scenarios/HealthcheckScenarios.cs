using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>healthcheck</c> golden scenarios.
    /// </summary>
    internal static class HealthcheckScenarios
    {
        /// <summary>
        /// The ways of writing a healthcheck: a shell string, the list form, durations as text and as
        /// <see cref="TimeSpan"/>, every setting together, and disabling the image's healthcheck.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("shell", service => service
                    .WithImage("nginx")
                    .WithHealthcheck("curl -f http://localhost || exit 1"))
                .WithService("list", service => service
                    .WithImage("nginx")
                    .WithHealthcheck(healthcheck => healthcheck.WithTest(new[] { "CMD", "curl", "-f", "http://localhost" })))
                .WithService("command", service => service
                    .WithImage("postgres")
                    .WithHealthcheck(healthcheck => healthcheck.WithCommand(new[] { "pg_isready", "-U", "postgres" })))
                .WithService("timespans", service => service
                    .WithImage("nginx")
                    .WithHealthcheck(healthcheck => healthcheck
                        .WithTest("curl -f http://localhost || exit 1")
                        .WithInterval(TimeSpan.FromSeconds(90))
                        .WithTimeout(TimeSpan.FromSeconds(10))
                        .WithStartPeriod(TimeSpan.FromMinutes(2))
                        .WithStartInterval(TimeSpan.FromMilliseconds(500))))
                .WithService("everything", service => service
                    .WithImage("nginx")
                    .WithHealthcheck(healthcheck => healthcheck
                        .WithTest(new[] { "CMD-SHELL", "curl -f http://localhost || exit 1" })
                        .WithInterval("1m30s")
                        .WithTimeout("10s")
                        .WithRetries(3)
                        .WithStartPeriod("40s")
                        .WithStartInterval("5s")))
                .WithService("none", service => service
                    .WithImage("nginx")
                    .WithHealthcheck(healthcheck => healthcheck.WithTest(new[] { "NONE" })))
                .WithService("disabled", service => service
                    .WithImage("nginx")
                    .WithoutHealthcheck())
                .Build();
        }
    }
}
