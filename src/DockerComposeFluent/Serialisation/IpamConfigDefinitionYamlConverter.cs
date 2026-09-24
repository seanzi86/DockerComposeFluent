using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes an <see cref="IpamConfigDefinition"/> as one address pool of an <c>ipam</c> mapping.
    /// </summary>
    internal sealed class IpamConfigDefinitionYamlConverter : YamlConverter<IpamConfigDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, IpamConfigDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalStringMap("aux_addresses", value.AuxAddresses);
            emitter.WriteOptionalScalar("gateway", value.Gateway);
            emitter.WriteOptionalScalar("ip_range", value.IpRange);
            emitter.WriteOptionalScalar("subnet", value.Subnet);
            emitter.EndMapping();
        }
    }
}
