using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    internal sealed class DockerComposeFileYamlConverter : YamlConverter<DockerComposeFile>
    {
        protected override void Write(IEmitter emitter, DockerComposeFile value, ObjectSerializer serialiser)
        {
            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "name", value.Name);
            YamlWriting.WriteMap(emitter, "services", value.Services, serialiser);
            YamlWriting.WriteMap(emitter, "networks", value.Networks, serialiser);
            YamlWriting.WriteMap(emitter, "volumes", value.Volumes, serialiser);
            YamlWriting.EndMapping(emitter);
        }
    }
}
