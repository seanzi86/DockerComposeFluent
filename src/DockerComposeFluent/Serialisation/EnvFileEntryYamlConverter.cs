using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes an <see cref="EnvFileEntry"/> as a plain path string when only the path is set, or as a mapping
    /// otherwise, since Compose accepts either shape for each entry independently.
    /// </summary>
    internal sealed class EnvFileEntryYamlConverter : YamlConverter<EnvFileEntry>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, EnvFileEntry value, ObjectSerializer serialiser)
        {
            if (value.IsPathOnly)
            {
                emitter.WriteScalar(value.Path!);
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalScalar("format", EnumNames.Of(value.Format));
            emitter.WriteOptionalScalar("path", value.Path);
            emitter.WriteOptionalBoolean("required", value.Required);
            emitter.EndMapping();
        }
    }
}
