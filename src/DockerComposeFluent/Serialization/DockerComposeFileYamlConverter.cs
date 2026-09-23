using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Serialization
{
    internal sealed class DockerComposeFileYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(DockerComposeFile);
        }

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            throw new NotSupportedException("Reading compose files is not supported.");
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            DockerComposeFile file = (DockerComposeFile)value!;

            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "name", file.Name);
            YamlWriting.WriteMap(emitter, "services", file.Services, serializer);
            YamlWriting.WriteMap(emitter, "networks", file.Networks, serializer);
            YamlWriting.WriteMap(emitter, "volumes", file.Volumes, serializer);
            YamlWriting.EndMapping(emitter);
        }
    }
}
