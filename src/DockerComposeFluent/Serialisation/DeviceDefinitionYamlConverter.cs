using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DeviceDefinition"/> as an entry of <c>deploy.resources.reservations.devices</c>.
    /// </summary>
    internal sealed class DeviceDefinitionYamlConverter : YamlConverter<DeviceDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DeviceDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalStringSequence("capabilities", value.Capabilities);
            emitter.WriteOptionalInteger("count", value.Count);
            emitter.WriteOptionalStringSequence("device_ids", value.DeviceIds);
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalStringMap("options", value.Options);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
