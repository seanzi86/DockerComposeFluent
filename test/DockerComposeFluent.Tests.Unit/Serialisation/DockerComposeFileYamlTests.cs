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

            Assert.Equal("{}\n", yaml);
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

            Assert.Equal("services:\n  \"web\":\n    image: \"nginx\"\n", yaml);
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

            Assert.Equal("networks:\n  \"default\": {}\n", yaml);
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

            Assert.Equal("name: \"" + value + "\"\n", Normalise(file.ToYaml()));
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

            Assert.Contains("services:\n  \"" + name + "\":\n", yaml);
            Assert.Contains("networks:\n  \"" + name + "\":\n", yaml);
            Assert.Contains("volumes:\n  \"" + name + "\":\n", yaml);
        }

        [Fact]
        public void ToYaml_EmptyArgument_IsWrittenAsEmptyString()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("app", service => service.WithCommand(new[] { "echo", "" }))
                .Build();

            Assert.Equal("services:\n  \"app\":\n    command: [\"echo\", \"\"]\n", Normalise(file.ToYaml()));
        }

        private static string Normalise(string yaml)
        {
            return yaml.Replace("\r\n", "\n");
        }
    }
}
