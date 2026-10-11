using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DevelopDefinition"/> as a service's <c>develop</c> entry.
    /// </summary>
    internal sealed class DevelopDefinitionYamlConverter : YamlConverter<DevelopDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DevelopDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalSequence("watch", value.Watch, serialiser);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
