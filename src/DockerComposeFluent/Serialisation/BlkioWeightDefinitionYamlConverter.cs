using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="BlkioWeightDefinition"/> as one device weight.
    /// </summary>
    internal sealed class BlkioWeightDefinitionYamlConverter : YamlConverter<BlkioWeightDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, BlkioWeightDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("path", value.Path);
            emitter.WriteOptionalInteger("weight", value.Weight);
            emitter.EndMapping();
        }
    }
}
