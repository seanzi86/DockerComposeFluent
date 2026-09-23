using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Serialisation
{
    internal sealed class VolumeDefinitionYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(VolumeDefinition);
        }

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            throw new NotSupportedException("Reading compose files is not supported.");
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            VolumeDefinition volume = (VolumeDefinition)value!;

            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "driver", volume.Driver);
            YamlWriting.WriteOptionalScalar(emitter, "name", volume.Name);
            YamlWriting.EndMapping(emitter);
        }
    }
}
