using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="NetworkAttachment"/> as the settings for one network of a service.
    /// </summary>
    internal sealed class NetworkAttachmentYamlConverter : YamlConverter<NetworkAttachment>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, NetworkAttachment value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalStringSequence("aliases", value.Aliases);
            emitter.WriteOptionalStringMap("driver_opts", value.DriverOptions);
            emitter.WriteOptionalInteger("gw_priority", value.GatewayPriority);
            emitter.WriteOptionalScalar("interface_name", value.InterfaceName);
            emitter.WriteOptionalScalar("ipv4_address", value.Ipv4Address);
            emitter.WriteOptionalScalar("ipv6_address", value.Ipv6Address);
            emitter.WriteOptionalStringSequence("link_local_ips", value.LinkLocalIps);
            emitter.WriteOptionalScalar("mac_address", value.MacAddress);
            emitter.WriteOptionalInteger("priority", value.Priority);
            emitter.EndMapping();
        }
    }
}
