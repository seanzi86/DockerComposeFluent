using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes an <see cref="IpamDefinition"/> as the <c>ipam</c> mapping of a network.
    /// </summary>
    internal sealed class IpamDefinitionYamlConverter : YamlConverter<IpamDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, IpamDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalSequence("config", value.Config, serialiser);
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalStringMap("options", value.Options);
            emitter.EndMapping();
        }
    }
}
