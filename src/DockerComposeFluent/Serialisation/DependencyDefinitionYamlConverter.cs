using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DependencyDefinition"/> as the long-syntax settings for one dependency. The
    /// <c>condition</c> is always written, because Compose rejects the long syntax without it.
    /// </summary>
    internal sealed class DependencyDefinitionYamlConverter : YamlConverter<DependencyDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DependencyDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteScalarEntry("condition", EnumNames.Of(value.Condition ?? DependencyCondition.ServiceStarted));
            emitter.WriteOptionalBoolean("required", value.Required);
            emitter.WriteOptionalBoolean("restart", value.Restart);
            emitter.EndMapping();
        }
    }
}
