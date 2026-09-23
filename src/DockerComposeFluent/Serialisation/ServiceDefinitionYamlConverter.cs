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
            emitter.WriteOptionalScalar("container_name", value.ContainerName);
            emitter.WriteOptionalScalar("image", value.Image);
            emitter.EndMapping();
        }
    }
}
