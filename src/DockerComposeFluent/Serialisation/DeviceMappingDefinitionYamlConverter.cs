using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DeviceMappingDefinition"/> as a short string when it can, or a mapping otherwise.
    /// </summary>
    internal sealed class DeviceMappingDefinitionYamlConverter : YamlConverter<DeviceMappingDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DeviceMappingDefinition value, ObjectSerializer serialiser)
        {
            if (value.CanBeShort)
            {
                string text = value.Source!;
                if (value.Target != null)
                {
                    text += ":" + value.Target;
                    if (value.Permissions != null)
                    {
                        text += ":" + value.Permissions;
                    }
                }

                emitter.WriteScalar(text);
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalScalar("permissions", value.Permissions);
            emitter.WriteOptionalScalar("source", value.Source);
            emitter.WriteOptionalScalar("target", value.Target);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
