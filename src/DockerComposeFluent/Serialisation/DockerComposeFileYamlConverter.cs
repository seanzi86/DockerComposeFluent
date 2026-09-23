using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DockerComposeFile"/> as the root compose mapping.
    /// </summary>
    internal sealed class DockerComposeFileYamlConverter : YamlConverter<DockerComposeFile>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DockerComposeFile value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.WriteMap("services", value.Services, serialiser);
            emitter.WriteMap("networks", value.Networks, serialiser);
            emitter.WriteMap("volumes", value.Volumes, serialiser);
            emitter.EndMapping();
        }
    }
}
