using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DiscreteResourceSpecDefinition"/> as a <c>discrete_resource_spec</c> entry.
    /// </summary>
    internal sealed class DiscreteResourceSpecDefinitionYamlConverter : YamlConverter<DiscreteResourceSpecDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DiscreteResourceSpecDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("kind", value.Kind);
            emitter.WriteOptionalInteger("value", value.Value);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
