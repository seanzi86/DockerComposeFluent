using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ResourceReservationsDefinition"/> as the <c>deploy.resources.reservations</c> entry.
    /// </summary>
    internal sealed class ResourceReservationsDefinitionYamlConverter : YamlConverter<ResourceReservationsDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ResourceReservationsDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("cpus", value.Cpus);
            emitter.WriteOptionalSequence("devices", value.Devices, serialiser);
            emitter.WriteOptionalSequence("generic_resources", value.GenericResources, serialiser);
            emitter.WriteOptionalScalar("memory", value.Memory);
            emitter.WriteOptionalInteger("pids", value.Pids);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
