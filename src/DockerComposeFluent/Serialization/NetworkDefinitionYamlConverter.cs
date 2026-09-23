using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Serialization
{
    internal sealed class NetworkDefinitionYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(NetworkDefinition);
        }

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            throw new NotSupportedException("Reading compose files is not supported.");
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            NetworkDefinition network = (NetworkDefinition)value!;

            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "driver", network.Driver);
            YamlWriting.WriteOptionalScalar(emitter, "name", network.Name);
            YamlWriting.EndMapping(emitter);
        }
    }
}
