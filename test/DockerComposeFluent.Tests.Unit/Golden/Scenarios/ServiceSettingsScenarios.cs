using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The common scalar service setting golden scenarios.
    /// </summary>
    internal static class ServiceSettingsScenarios
    {
        /// <summary>
        /// Every plain string, boolean, integer and duration setting on a service, using both the text and
        /// <see cref="TimeSpan"/> forms of the durations.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("everything", service => service
                    .WithImage("example/app")
                    .WithHostname("app-host")
                    .WithDomainName("example.com")
                    .WithUser("1000:1000")
                    .WithWorkingDir("/srv/app")
                    .WithMacAddress("02:42:ac:11:00:02")
                    .WithStopSignal("SIGINT")
                    .WithStopGracePeriod("1m30s")
                    .WithInit(true)
                    .WithPrivileged(false)
                    .WithReadOnly(true)
                    .WithStdinOpen(true)
                    .WithTty(true)
                    .WithRuntime("runc")
                    .WithPullPolicy("every_12h")
                    .WithPullRefreshAfter("24h")
                    .WithIsolation("default")
                    .WithAttach(false)
                    .WithScale(3)
                    .WithUseApiSocket(false))
                .WithService("timespans", service => service
                    .WithImage("example/app")
                    .WithStopGracePeriod(TimeSpan.FromSeconds(45))
                    .WithPullPolicy("missing")
                    .WithPullRefreshAfter(TimeSpan.FromHours(6)))
                .Build();
        }
    }
}
