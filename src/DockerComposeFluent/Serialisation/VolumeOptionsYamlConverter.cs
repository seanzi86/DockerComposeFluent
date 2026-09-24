using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes <see cref="VolumeOptions"/> as the <c>volume</c> mapping of a mount.
    /// </summary>
    internal sealed class VolumeOptionsYamlConverter : YamlConverter<VolumeOptions>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, VolumeOptions value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalBoolean("nocopy", value.NoCopy);
            emitter.WriteOptionalScalar("subpath", value.Subpath);
            emitter.EndMapping();
        }
    }
}
