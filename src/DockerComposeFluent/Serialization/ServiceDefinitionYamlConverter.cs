using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Serialization
{
    internal sealed class ServiceDefinitionYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(ServiceDefinition);
        }

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            throw new NotSupportedException("Reading compose files is not supported.");
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            ServiceDefinition service = (ServiceDefinition)value!;

            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "container_name", service.ContainerName);
            YamlWriting.WriteOptionalScalar(emitter, "image", service.Image);
            YamlWriting.EndMapping(emitter);
        }
    }
}
