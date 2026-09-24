using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="MountDefinition"/> as a long-syntax mount.
    /// </summary>
    internal sealed class MountDefinitionYamlConverter : YamlConverter<MountDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, MountDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalValue("bind", value.Bind, serialiser);
            emitter.WriteOptionalScalar("consistency", value.Consistency);
            emitter.WriteOptionalValue("image", value.Image, serialiser);
            emitter.WriteOptionalBoolean("read_only", value.ReadOnly);
            emitter.WriteOptionalScalar("source", value.Source);
            emitter.WriteScalarEntry("target", value.Target);
            emitter.WriteOptionalValue("tmpfs", value.Tmpfs, serialiser);
            emitter.WriteScalarEntry("type", EnumNames.Of(value.Type));
            emitter.WriteOptionalValue("volume", value.Volume, serialiser);
            emitter.EndMapping();
        }
    }
}
