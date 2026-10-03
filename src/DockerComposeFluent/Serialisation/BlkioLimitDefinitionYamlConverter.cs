using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="BlkioLimitDefinition"/> as one device rate limit.
    /// </summary>
    internal sealed class BlkioLimitDefinitionYamlConverter : YamlConverter<BlkioLimitDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, BlkioLimitDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("path", value.Path);
            emitter.WriteOptionalScalar("rate", value.Rate);
            emitter.EndMapping();
        }
    }
}
