using System.Collections.Generic;
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

        /// <summary>
        /// Every way of attaching a service to a network and of defining a network, in one file.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile KitchenSink()
        {
            return new DockerComposeBuilder()
                .WithService("list", service => service.WithImage("nginx").WithNetworks(new[] { "front", "back" }))
                .WithService("single", service => service.WithImage("nginx").WithNetwork("front"))
                .WithService("aliased", service => service
                    .WithImage("busybox")
                    .WithNetwork("front", network => network.WithAlias("alias1").WithAlias("alias3"))
                    .WithNetwork("back", network => network.WithAliases(new[] { "alias2" })))
                .WithService("addressed", service => service
                    .WithImage("busybox")
                    .WithNetwork("back", network => network
                        .WithIpv4Address("172.16.238.10")
                        .WithIpv6Address("2001:3984:3989::10")
                        .WithLinkLocalIps(new[] { "57.123.22.11", "57.123.22.13" })
                        .WithMacAddress("02:42:ac:11:00:02")
                        .WithPriority(1000)
                        .WithDriverOptions(new Dictionary<string, string> { ["foo"] = "bar", ["baz"] = "1" })))
                .WithService("ordered", service => service
                    .WithImage("busybox")
                    .WithNetwork("front", network => network.WithGatewayPriority(1).WithInterfaceName("eth0"))
                    .WithNetwork("back"))
                .WithNetwork("front", network => network
                    .WithDriver("overlay")
                    .WithAttachable(true)
                    .WithDriverOption("com.docker.network.driver.mtu", "1400")
                    .WithName("front_net"))
                .WithNetwork("back", network => network
                    .WithInternal(true)
                    .WithEnableIpv4(true)
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
                .WithNetwork("outside", network => network.WithExternal(true).WithName("shared_outside"))
                .Build();
        }
    }
}
