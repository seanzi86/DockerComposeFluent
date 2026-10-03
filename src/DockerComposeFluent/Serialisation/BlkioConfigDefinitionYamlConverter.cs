using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="BlkioConfigDefinition"/> as a service's <c>blkio_config</c> entry.
    /// </summary>
    internal sealed class BlkioConfigDefinitionYamlConverter : YamlConverter<BlkioConfigDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, BlkioConfigDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalSequence("device_read_bps", value.DeviceReadBps, serialiser);
            emitter.WriteOptionalSequence("device_read_iops", value.DeviceReadIops, serialiser);
            emitter.WriteOptionalSequence("device_write_bps", value.DeviceWriteBps, serialiser);
            emitter.WriteOptionalSequence("device_write_iops", value.DeviceWriteIops, serialiser);
            emitter.WriteOptionalInteger("weight", value.Weight);
            emitter.WriteOptionalSequence("weight_device", value.WeightDevice, serialiser);
            emitter.EndMapping();
        }
    }
}
