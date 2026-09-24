using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="PortDefinition"/> as a long-syntax port mapping.
    /// </summary>
    internal sealed class PortDefinitionYamlConverter : YamlConverter<PortDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, PortDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("app_protocol", value.AppProtocol);
            emitter.WriteOptionalScalar("host_ip", value.HostIp);
            emitter.WriteOptionalScalar("mode", EnumNames.Of(value.Mode));
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.WriteOptionalScalar("protocol", EnumNames.Of(value.Protocol));
            emitter.WriteOptionalScalar("published", value.Published);
            emitter.WriteInteger("target", value.Target);
            emitter.EndMapping();
        }
    }
}
