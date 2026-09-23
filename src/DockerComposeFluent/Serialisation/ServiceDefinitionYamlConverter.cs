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
            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "container_name", value.ContainerName);
            YamlWriting.WriteOptionalScalar(emitter, "image", value.Image);
            YamlWriting.EndMapping(emitter);
        }
    }
}
