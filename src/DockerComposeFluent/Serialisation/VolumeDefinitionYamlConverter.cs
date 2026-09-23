using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    internal sealed class VolumeDefinitionYamlConverter : YamlConverter<VolumeDefinition>
    {
        protected override void Write(IEmitter emitter, VolumeDefinition value, ObjectSerializer serialiser)
        {
            YamlWriting.StartMapping(emitter);
            YamlWriting.WriteOptionalScalar(emitter, "driver", value.Driver);
            YamlWriting.WriteOptionalScalar(emitter, "name", value.Name);
            YamlWriting.EndMapping(emitter);
        }
    }
}
