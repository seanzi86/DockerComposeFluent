using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes <see cref="ImageOptions"/> as the <c>image</c> mapping of a mount.
    /// </summary>
    internal sealed class ImageOptionsYamlConverter : YamlConverter<ImageOptions>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ImageOptions value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("subpath", value.Subpath);
            emitter.EndMapping();
        }
    }
}
