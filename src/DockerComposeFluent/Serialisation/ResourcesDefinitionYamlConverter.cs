using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ResourcesDefinition"/> as the <c>deploy.resources</c> entry.
    /// </summary>
    internal sealed class ResourcesDefinitionYamlConverter : YamlConverter<ResourcesDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ResourcesDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalValue("limits", value.Limits, serialiser);
            emitter.WriteOptionalValue("reservations", value.Reservations, serialiser);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
