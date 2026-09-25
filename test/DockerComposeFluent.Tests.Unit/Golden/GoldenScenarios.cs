using System;
using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    /// <summary>
    /// The catalogue of compose files whose YAML is checked against fixtures. To add one, add an entry here
    /// and accept its Verify fixture (see CONTRIBUTING.md).
    /// </summary>
    internal static class GoldenScenarios
    {
        internal static IReadOnlyDictionary<string, Func<DockerComposeFile>> All { get; } =
            new Dictionary<string, Func<DockerComposeFile>>
            {
                ["basic"] = Basic,
                ["commands"] = Commands,
                ["environment"] = Environment,
                ["networks"] = Networks,
                ["ports"] = Ports,
                ["readme"] = Readme,
                ["volumes"] = Volumes
            };

        private static DockerComposeFile Basic()
        {
            return new DockerComposeBuilder()
                .WithName("demo")
                .WithService("web", service => service.WithContainerName("web-container").WithImage("nginx:latest"))
                .WithNetwork("frontend", network => network.WithDriver("bridge"))
                .WithVolume("data", volume => volume.WithDriver("local").WithName("demo_data"))
                .Build();
        }

        private static DockerComposeFile Commands()
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

        private static DockerComposeFile Environment()
        {
            return new DockerComposeBuilder()
                .WithService("map", service => service
                    .WithImage("nginx")
                    .WithEnvironment("RACK_ENV", "development")
                    .WithEnvironment("SHOW", true)
                    .WithEnvironment("PORT", 8080)
                    .WithEnvironment("EMPTY", "")
                    .WithEnvironment("USER_INPUT"))
                .WithService("list", service => service
                    .WithImage("nginx")
                    .WithEnvironment(new[] { "RACK_ENV=development", "SHOW=true", "URL=http://x?a=b", "EMPTY=", "USER_INPUT" }))
                .WithService("mixed", service => service
                    .WithImage("nginx")
                    .WithEnvironment("A", "1")
                    .WithEnvironment("B")
                    .WithEnvironment(new[] { "C=3" }))
                .Build();
        }

        private static DockerComposeFile Networks()
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

        private static DockerComposeFile Ports()
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

        private static DockerComposeFile Readme()
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

        private static DockerComposeFile Volumes()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("nginx")
                    .WithVolume("db-data:/var/lib/db")
                    .WithVolume("./src", "/app", readOnly: true)
                    .WithVolume(mount => mount
                        .WithType(MountType.Bind)
                        .WithSource("./conf")
                        .WithTarget("/etc/app")
                        .WithBindOptions(options => options
                            .WithCreateHostPath(false)
                            .WithPropagation(BindPropagation.RShared)
                            .WithSelinux(SelinuxLabel.Shared)))
                    .WithVolume(mount => mount
                        .WithType(MountType.Volume)
                        .WithSource("cache")
                        .WithTarget("/cache")
                        .WithReadOnly(true)
                        .WithVolumeOptions(options => options.WithNoCopy(true).WithSubpath("sub")))
                    .WithVolume(mount => mount
                        .WithType(MountType.Tmpfs)
                        .WithTarget("/tmp")
                        .WithTmpfsOptions(options => options.WithSize("100m").WithMode(Convert.ToInt32("1777", 8))))
                    .WithVolume(mount => mount
                        .WithType(MountType.Image)
                        .WithSource("example/assets:1")
                        .WithTarget("/assets")
                        .WithImageOptions(options => options.WithSubpath("static"))))
                .WithVolume("db-data", volume => volume.WithDriver("local"))
                .WithVolume("cache", volume => volume.WithName("app_cache"))
                .Build();
        }
    }
}
