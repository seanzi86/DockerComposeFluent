using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ConfigReference"/> as a plain source-name string when only the source is set, or as
    /// a mapping otherwise, since Compose accepts either shape for each entry independently.
    /// </summary>
    internal sealed class ConfigReferenceYamlConverter : YamlConverter<ConfigReference>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ConfigReference value, ObjectSerializer serialiser)
        {
            if (value.IsSourceOnly)
            {
                emitter.WriteScalar(value.Source!);
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalScalar("gid", value.Gid);
            emitter.WriteOptionalInteger("mode", value.Mode);
            emitter.WriteOptionalScalar("source", value.Source);
            emitter.WriteOptionalScalar("target", value.Target);
            emitter.WriteOptionalScalar("uid", value.Uid);
            emitter.EndMapping();
        }
    }
}
