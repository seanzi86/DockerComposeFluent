using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ServiceDefinition"/> as a service entry.
    /// </summary>
    internal sealed class ServiceDefinitionYamlConverter : YamlConverter<ServiceDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ServiceDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalValue("command", value.Command, serialiser);
            emitter.WriteOptionalScalar("container_name", value.ContainerName);
            emitter.WriteOptionalValue("entrypoint", value.Entrypoint, serialiser);
            emitter.WriteOptionalScalar("image", value.Image);
            emitter.WriteOptionalSequence("ports", value.Ports, serialiser);
            emitter.WriteOptionalSequence("volumes", value.Volumes, serialiser);
            emitter.EndMapping();
        }
    }
}
