using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="NetworkDefinition"/> as a network entry.
    /// </summary>
    internal sealed class NetworkDefinitionYamlConverter : YamlConverter<NetworkDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, NetworkDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalBoolean("attachable", value.Attachable);
            emitter.WriteOptionalScalar("driver", value.Driver);
            emitter.WriteOptionalStringMap("driver_opts", value.DriverOptions);
            emitter.WriteOptionalBoolean("enable_ipv4", value.EnableIpv4);
            emitter.WriteOptionalBoolean("enable_ipv6", value.EnableIpv6);
            emitter.WriteOptionalBoolean("external", value.External);
            emitter.WriteOptionalBoolean("internal", value.Internal);
            emitter.WriteOptionalValue("ipam", value.Ipam, serialiser);
            emitter.WriteOptionalScalar("name", value.Name);
            emitter.EndMapping();
        }
    }
}
