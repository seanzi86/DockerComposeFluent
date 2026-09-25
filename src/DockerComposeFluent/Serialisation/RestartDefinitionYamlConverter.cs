using System.Globalization;
using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="RestartDefinition"/> as its compose-spec string, such as <c>"always"</c> or
    /// <c>"on-failure:5"</c>. The value is quoted, which matters for <c>"no"</c>: unquoted, YAML reads it as false.
    /// </summary>
    internal sealed class RestartDefinitionYamlConverter : YamlConverter<RestartDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, RestartDefinition value, ObjectSerializer serialiser)
        {
            string text = EnumNames.Of(value.Policy);
            if (value.MaxRetries != null)
            {
                text += ":" + value.MaxRetries.Value.ToString(CultureInfo.InvariantCulture);
            }

            emitter.WriteScalar(text);
        }
    }
}
