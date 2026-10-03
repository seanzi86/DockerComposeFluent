using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="GenericResourceDefinition"/> as one generic resource.
    /// </summary>
    internal sealed class GenericResourceDefinitionYamlConverter : YamlConverter<GenericResourceDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, GenericResourceDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalValue("discrete_resource_spec", value.DiscreteResourceSpec, serialiser);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
