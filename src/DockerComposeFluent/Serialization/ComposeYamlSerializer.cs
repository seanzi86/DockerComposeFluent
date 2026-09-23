using DockerComposeFluent.Models;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialization
{
    internal static class ComposeYamlSerializer
    {
        private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithTypeConverter(new DockerComposeFileYamlConverter())
            .WithTypeConverter(new ServiceDefinitionYamlConverter())
            .WithTypeConverter(new NetworkDefinitionYamlConverter())
            .WithTypeConverter(new VolumeDefinitionYamlConverter())
            .Build();

        internal static string Serialize(DockerComposeFile file)
        {
            return Serializer.Serialize(file);
        }
    }
}
