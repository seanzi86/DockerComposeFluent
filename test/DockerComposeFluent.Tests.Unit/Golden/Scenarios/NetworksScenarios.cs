using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>networks</c> golden scenarios.
    /// </summary>
    internal static class NetworksScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("web", service => service.WithImage("nginx").WithNetworks(new[] { "front", "back" }))
                .WithService("app", service => service
                    .WithImage("busybox")
                    .WithNetwork("front")
                    .WithNetwork("back", network => network
                        .WithAliases(new[] { "db", "database" })
                        .WithDriverOption("foo", "bar")
                        .WithIpv4Address("172.16.238.10")
                        .WithIpv6Address("2001:3984:3989::10")
                        .WithLinkLocalIp("57.123.22.11")
                        .WithMacAddress("02:42:ac:11:00:02")
                        .WithPriority(100)))
                .WithNetwork("front", network => network
                    .WithAttachable(true)
                    .WithDriver("overlay")
                    .WithDriverOption("com.docker.network.driver.mtu", "1400")
                    .WithInternal(false)
                    .WithName("front_net"))
                .WithNetwork("back", network => network
                    .WithEnableIpv6(true)
                    .WithIpam(ipam => ipam
                        .WithDriver("default")
                        .WithConfig(config => config
                            .WithSubnet("172.16.238.0/24")
                            .WithIpRange("172.16.238.0/25")
                            .WithGateway("172.16.238.254")
                            .WithAuxAddress("host1", "172.16.238.5"))
                        .WithConfig("2001:3984:3989::/64")
                        .WithOption("foo", "bar")))
                .WithNetwork("outside", network => network.WithExternal(true))
                .Build();
        }
    }
}
