using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="NetworkDefinition"/> as a network entry.
    /// </summary>
    internal sealed class NetworkDefinitionYamlConverter : YamlConverter<NetworkDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, NetworkDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.EndMapping();
        }
    }
}
