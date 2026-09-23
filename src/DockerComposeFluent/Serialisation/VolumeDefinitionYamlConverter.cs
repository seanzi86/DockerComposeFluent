using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="VolumeDefinition"/> as a volume entry.
    /// </summary>
    internal sealed class VolumeDefinitionYamlConverter : YamlConverter<VolumeDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, VolumeDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.EndMapping();
        }
    }
}
