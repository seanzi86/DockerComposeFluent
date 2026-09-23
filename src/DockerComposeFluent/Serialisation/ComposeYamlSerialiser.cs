using DockerComposeFluent.Models;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    internal static class ComposeYamlSerialiser
    {
        private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithTypeConverter(new DockerComposeFileYamlConverter())
            .WithTypeConverter(new ServiceDefinitionYamlConverter())
            .WithTypeConverter(new NetworkDefinitionYamlConverter())
            .WithTypeConverter(new VolumeDefinitionYamlConverter())
            .Build();

        internal static string Serialise(DockerComposeFile file)
        {
            return Serializer.Serialize(file);
        }
    }
}
