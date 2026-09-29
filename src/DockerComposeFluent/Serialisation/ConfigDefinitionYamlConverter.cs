using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ConfigDefinition"/> as a top-level <c>configs</c> entry.
    /// </summary>
    internal sealed class ConfigDefinitionYamlConverter : YamlConverter<ConfigDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ConfigDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("content", value.Content);
            emitter.WriteOptionalBoolean("external", value.External);
            emitter.WriteOptionalScalar("environment", value.Environment);
            emitter.WriteOptionalScalar("file", value.File);
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.EndMapping();
        }
    }
}
