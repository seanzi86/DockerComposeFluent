using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes an <see cref="IncludeDefinition"/> as a plain path string when only a single path is set, or as
    /// a mapping otherwise.
    /// </summary>
    internal sealed class IncludeDefinitionYamlConverter : YamlConverter<IncludeDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, IncludeDefinition value, ObjectSerializer serialiser)
        {
            if (value.IsPathOnly)
            {
                emitter.WriteScalar(value.Path[0]);
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalStringSequence("env_file", value.EnvFile);
            emitter.WriteOptionalStringSequence("path", value.Path);
            emitter.WriteOptionalScalar("project_directory", value.ProjectDirectory);
            emitter.EndMapping();
        }
    }
}
