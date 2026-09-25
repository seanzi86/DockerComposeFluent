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

            if (value.DependsOn.Count > 0)
            {
                emitter.WriteOptionalValue("depends_on", value.DependsOn, serialiser);
            }

            emitter.WriteOptionalValue("entrypoint", value.Entrypoint, serialiser);

            if (value.Environment.Variables.Count > 0)
            {
                emitter.WriteOptionalValue("environment", value.Environment, serialiser);
            }

            emitter.WriteOptionalScalar("image", value.Image);

            if (value.Networks.Count > 0)
            {
                emitter.WriteOptionalValue("networks", value.Networks, serialiser);
            }

            emitter.WriteOptionalSequence("ports", value.Ports, serialiser);
            emitter.WriteOptionalValue("restart", value.Restart, serialiser);
            emitter.WriteOptionalSequence("volumes", value.Volumes, serialiser);
            emitter.EndMapping();
        }
    }
}
