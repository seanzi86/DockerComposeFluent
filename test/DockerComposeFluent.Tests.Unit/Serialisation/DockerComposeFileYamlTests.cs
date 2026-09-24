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
