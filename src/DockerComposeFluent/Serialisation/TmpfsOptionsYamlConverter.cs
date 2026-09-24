using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes <see cref="TmpfsOptions"/> as the <c>tmpfs</c> mapping of a mount.
    /// </summary>
    internal sealed class TmpfsOptionsYamlConverter : YamlConverter<TmpfsOptions>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, TmpfsOptions value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalInteger("mode", value.Mode);
            emitter.WriteOptionalScalar("size", value.Size);
            emitter.EndMapping();
        }
    }
}
