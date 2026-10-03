using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="SecretDefinition"/> as a top-level <c>secrets</c> entry.
    /// </summary>
    internal sealed class SecretDefinitionYamlConverter : YamlConverter<SecretDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, SecretDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalStringMap("driver_opts", value.DriverOptions);
            emitter.WriteOptionalBoolean("external", value.External);
            emitter.WriteOptionalScalar("environment", value.Environment);
            emitter.WriteOptionalScalar("file", value.File);

            if (value.Labels.Values.Count > 0)
            {
                emitter.WriteOptionalValue("labels", value.Labels, serialiser);
            }

            emitter.WriteOptionalScalar("name", value.Name);
            emitter.WriteOptionalScalar("template_driver", value.TemplateDriver);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
