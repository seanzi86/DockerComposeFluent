using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="GpuDefinition"/> as one entry of a service's <c>gpus</c> list.
    /// </summary>
    internal sealed class GpuDefinitionYamlConverter : YamlConverter<GpuDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, GpuDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalStringSequence("capabilities", value.Capabilities);
            emitter.WriteOptionalInteger("count", value.Count);
            emitter.WriteOptionalStringSequence("device_ids", value.DeviceIds);
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalStringMap("options", value.Options);
            emitter.EndMapping();
        }
    }
}
