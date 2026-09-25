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

        /// <summary>
        /// Every way of writing a port in one file: short syntax with ranges, a host IP and protocols, and the long
        /// syntax with each field.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile KitchenSink()
        {
            return new DockerComposeBuilder()
                .WithService("short", service => service
                    .WithImage("nginx")
                    .WithPorts(new[] { "3000", "3000-3005", "8000:8000", "9090-9091:8080-8081", "49100:22", "8000-9000:80" })
                    .WithPort("127.0.0.1:8001:8001")
                    .WithPort("127.0.0.1:5000-5010:5000-5010")
                    .WithPort("6060:6060/udp"))
                .WithService("helpers", service => service
                    .WithImage("nginx")
                    .WithPort(80)
                    .WithPort(8080, 80)
                    .WithPort(5353, 53, PortProtocol.Udp)
                    .WithPort(8443, 443, PortProtocol.Tcp))
                .WithService("long", service => service
                    .WithImage("nginx")
                    .WithPort(port => port.WithTarget(80))
                    .WithPort(port => port.WithTarget(80).WithPublished(8080))
                    .WithPort(port => port.WithTarget(80).WithPublished(8000, 8010).WithHostIp("127.0.0.1"))
                    .WithPort(port => port.WithTarget(53).WithPublished(5353).WithProtocol(PortProtocol.Udp))
                    .WithPort(port => port
                        .WithTarget(443)
                        .WithPublished(8443)
                        .WithHostIp("0.0.0.0")
                        .WithProtocol(PortProtocol.Tcp)
                        .WithMode(PortMode.Ingress)
                        .WithAppProtocol("https")
                        .WithName("secure")))
                .Build();
        }
    }
}
