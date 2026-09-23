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
            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "driver", value.Driver);
            YamlWriting.WriteOptionalScalar(emitter, "name", value.Name);
            YamlWriting.EndMapping(emitter);
        }
    }
}
