using System;
using System.Collections.Generic;
using System.IO;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Serialisation
{
    public class DockerComposeFileYamlTests
    {
        [Fact]
        public void ToYaml_EmptyFile_ProducesEmptyMapping()
        {
            DockerComposeFile file = new();

            string yaml = Normalise(file.ToYaml());

            Assert.Equal("{}", yaml);
        }

        [Fact]
        public void ToYaml_OmitsUnsetProperties()
        {
            DockerComposeFile file = new()
            {
                Services = new Dictionary<string, ServiceDefinition>
                {
                    ["web"] = new ServiceDefinition { Image = "nginx" }
                }
            };

            string yaml = Normalise(file.ToYaml());

            Assert.Equal(
                """
                services:
                  "web":
                    image: "nginx"
                """,
                yaml);
        }

        [Fact]
        public void ToYaml_EmptyDefinitions_EmitEmptyMappings()
        {
            DockerComposeFile file = new()
            {
                Networks = new Dictionary<string, NetworkDefinition>
                {
                    ["default"] = new NetworkDefinition()
                }
            };

            string yaml = Normalise(file.ToYaml());

            Assert.Equal(
                """
                networks:
                  "default": {}
                """,
                yaml);
        }

        [Fact]
        public void ToYaml_FullFile_MatchesGoldenFixture()
        {
            DockerComposeFile file = new()
            {
                Name = "demo",
                Services = new Dictionary<string, ServiceDefinition>
                {
                    ["web"] = new ServiceDefinition { ContainerName = "web-container", Image = "nginx:latest" }
                },
                Networks = new Dictionary<string, NetworkDefinition>
                {
                    ["frontend"] = new NetworkDefinition { Driver = "bridge" }
                },
                Volumes = new Dictionary<string, VolumeDefinition>
                {
                    ["data"] = new VolumeDefinition { Driver = "local", Name = "demo_data" }
                }
            };

            string expected = Normalise(File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "basic.yml")));

            Assert.Equal(expected, Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_CommandAndEntrypoint_MatchGoldenFixture()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("node:22")
                    .WithCommand(new[] { "node", "server.js", "--port", "8080" })
                    .WithEntrypoint(Array.Empty<string>()))
                .WithService("shell", service => service
                    .WithImage("node:22")
                    .WithCommand("npm start")
                    .WithEntrypoint("/docker-entrypoint.sh"))
                .Build();

            string expected = Normalise(File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "commands.yml")));

            Assert.Equal(expected, Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_Ports_MatchGoldenFixture()
        {
            DockerComposeFile file = new DockerComposeBuilder()
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

            string expected = Normalise(File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "ports.yml")));

            Assert.Equal(expected, Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_Volumes_MatchGoldenFixture()
        {
            DockerComposeFile file = new DockerComposeBuilder()
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

            string expected = Normalise(File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "volumes.yml")));

            Assert.Equal(expected, Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_Networks_MatchGoldenFixture()
        {
            DockerComposeFile file = new DockerComposeBuilder()
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

            string expected = Normalise(File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "networks.yml")));

            Assert.Equal(expected, Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_NewerComposeNetworkFields_AreWritten()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("app", service => service.WithNetwork("net", network => network.WithGatewayPriority(1).WithInterfaceName("eth0")))
                .WithNetwork("net", network => network.WithEnableIpv4(false))
                .Build();

            Assert.Equal(
                """
                services:
                  "app":
                    networks:
                      "net":
                        gw_priority: 1
                        interface_name: "eth0"
                networks:
                  "net":
                    enable_ipv4: false
                """,
                Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_Environment_MatchGoldenFixture()
        {
            DockerComposeFile file = new DockerComposeBuilder()
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

            string expected = Normalise(File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "environment.yml")));

            Assert.Equal(expected, Normalise(file.ToYaml()));
        }

        [Theory]
        [InlineData("true")]
        [InlineData("False")]
        [InlineData("yes")]
        [InlineData("null")]
        [InlineData("~")]
        [InlineData("123")]
        [InlineData("1.5")]
        [InlineData("1e3")]
        [InlineData("0x1F")]
        [InlineData("2024-01-15")]
        [InlineData("nginx")]
        [InlineData("-g")]
        [InlineData("v1.2.3")]
        public void ToYaml_StringValues_AreAlwaysDoubleQuoted(string value)
        {
            DockerComposeFile file = new DockerComposeBuilder().WithName(value).Build();

            Assert.Equal(
                $"""
                name: "{value}"
                """,
                Normalise(file.ToYaml()));
        }

        [Theory]
        [InlineData("web")]
        [InlineData("123")]
        [InlineData("true")]
        [InlineData("null")]
        [InlineData("1e3")]
        [InlineData("2024-01-15")]
        public void ToYaml_UserSuppliedNames_AreAlwaysDoubleQuoted(string name)
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService(name, service => service.WithImage("nginx"))
                .WithNetwork(name, network => network.WithDriver("bridge"))
                .WithVolume(name, volume => volume.WithDriver("local"))
                .Build();

            string yaml = Normalise(file.ToYaml());

            Assert.Contains(
                $"""
                services:
                  "{name}":
                """,
                yaml);
            Assert.Contains(
                $"""
                networks:
                  "{name}":
                """,
                yaml);
            Assert.Contains(
                $"""
                volumes:
                  "{name}":
                """,
                yaml);
        }

        [Fact]
        public void ToYaml_EmptyArgument_IsWrittenAsEmptyString()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("app", service => service.WithCommand(new[] { "echo", "" }))
                .Build();

            Assert.Equal(
                """
                services:
                  "app":
                    command: ["echo", ""]
                """,
                Normalise(file.ToYaml()));
        }

        private static string Normalise(string yaml)
        {
            return yaml.Replace("\r\n", "\n").TrimEnd('\n');
        }
    }
}
