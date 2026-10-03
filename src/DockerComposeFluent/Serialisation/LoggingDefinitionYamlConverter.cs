using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="LoggingDefinition"/> as a service's <c>logging</c> entry.
    /// </summary>
    internal sealed class LoggingDefinitionYamlConverter : YamlConverter<LoggingDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, LoggingDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalStringMap("options", value.Options);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
