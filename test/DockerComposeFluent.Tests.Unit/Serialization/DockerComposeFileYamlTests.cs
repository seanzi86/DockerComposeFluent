using System.Collections.Generic;
using System.IO;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Serialization
{
    public class DockerComposeFileYamlTests
    {
        [Fact]
        public void ToYaml_EmptyFile_ProducesEmptyMapping()
        {
            DockerComposeFile file = new();

            string yaml = Normalize(file.ToYaml());

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

            string yaml = Normalize(file.ToYaml());

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

            string yaml = Normalize(file.ToYaml());

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

            string expected = Normalize(File.ReadAllText(Path.Combine("Serialization", "Fixtures", "basic.yml")));

            Assert.Equal(expected, Normalize(file.ToYaml()));
        }

        private static string Normalize(string yaml)
        {
            return yaml.Replace("\r\n", "\n");
        }
    }
}
