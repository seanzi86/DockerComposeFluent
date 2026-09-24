using System;
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
            emitter.WriteOptionalScalar("mode", value.Mode.HasValue ? ModeName(value.Mode.Value) : null);
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.WriteOptionalScalar("protocol", value.Protocol.HasValue ? ProtocolName(value.Protocol.Value) : null);
            emitter.WriteOptionalScalar("published", value.Published);
            emitter.WriteInteger("target", value.Target);
            emitter.EndMapping();
        }

        private static string ModeName(PortMode mode)
        {
            switch (mode)
            {
                case PortMode.Host:
                    return "host";
                case PortMode.Ingress:
                    return "ingress";
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown port mode.");
            }
        }

        private static string ProtocolName(PortProtocol protocol)
        {
            switch (protocol)
            {
                case PortProtocol.Tcp:
                    return "tcp";
                case PortProtocol.Udp:
                    return "udp";
                default:
                    throw new ArgumentOutOfRangeException(nameof(protocol), protocol, "Unknown port protocol.");
            }
        }
    }
}
