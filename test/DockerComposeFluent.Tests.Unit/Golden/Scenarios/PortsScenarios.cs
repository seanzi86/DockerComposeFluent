using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>ports</c> golden scenarios.
    /// </summary>
    internal static class PortsScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("web", service => service
                    .WithImage("nginx")
                    .WithPorts(new[] { "80", "8080:80" })
                    .WithPort("127.0.0.1:9000:90/udp")
                    .WithPort(83)
                    .WithPort(8081, 81, PortProtocol.Udp)
                    .WithPort(port => port
                        .WithTarget(82)
                        .WithPublished(8000, 9000)
                        .WithHostIp("127.0.0.1")
                        .WithMode(PortMode.Host)
                        .WithAppProtocol("http")
                        .WithName("web")))
                .WithService("worker", service => service.WithImage("busybox"))
                .Build();
        }
    }
}
