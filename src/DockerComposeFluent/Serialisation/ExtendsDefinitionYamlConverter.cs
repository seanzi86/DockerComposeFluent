using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes an <see cref="ExtendsDefinition"/> as a plain service-name string when only the service is set,
    /// or as a mapping otherwise.
    /// </summary>
    internal sealed class ExtendsDefinitionYamlConverter : YamlConverter<ExtendsDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ExtendsDefinition value, ObjectSerializer serialiser)
        {
            if (value.IsServiceOnly)
            {
                emitter.WriteScalar(value.Service!);
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalScalar("file", value.File);
            emitter.WriteOptionalScalar("service", value.Service);
            emitter.EndMapping();
        }
    }
}
