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

            Assert.Equal("services:\n  web:\n    image: nginx\n", yaml);
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

            Assert.Equal("networks:\n  default: {}\n", yaml);
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
        [InlineData("true", "\"true\"")]
        [InlineData("False", "\"False\"")]
        [InlineData("yes", "\"yes\"")]
        [InlineData("null", "\"null\"")]
        [InlineData("~", "\"~\"")]
        [InlineData("123", "\"123\"")]
        [InlineData("1.5", "\"1.5\"")]
        [InlineData("0x1F", "\"0x1F\"")]
        [InlineData("nginx", "nginx")]
        [InlineData("-g", "-g")]
        [InlineData("v1.2.3", "v1.2.3")]
        public void ToYaml_StringsThatLookLikeOtherTypes_AreQuoted(string value, string expected)
        {
            DockerComposeFile file = new DockerComposeBuilder().WithName(value).Build();

            Assert.Equal("name: " + expected + "\n", Normalise(file.ToYaml()));
        }

        [Fact]
        public void ToYaml_EmptyArgument_IsWrittenAsEmptyString()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("app", service => service.WithCommand(new[] { "echo", "" }))
                .Build();

            Assert.Equal("services:\n  app:\n    command: [echo, \"\"]\n", Normalise(file.ToYaml()));
        }

        private static string Normalise(string yaml)
        {
            return yaml.Replace("\r\n", "\n");
        }
    }
}
