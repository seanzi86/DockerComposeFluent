using System;
using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;
using DockerComposeFluent.Tests.Unit.Golden;

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
        public void ToYaml_RawModels_MatchGoldenFixture()
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

            GoldenFile.AssertMatches("basic", file);
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
            return GoldenFile.Normalise(yaml);
        }
    }
}
