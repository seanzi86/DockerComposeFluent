using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds an <see cref="IpamConfigDefinition"/>: one address pool of an IPAM configuration.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
    /// </summary>
    public sealed class IpamConfigBuilder
    {
        private IpamConfigDefinition _config = new IpamConfigDefinition();

        /// <summary>
        /// Sets the subnet.
        /// </summary>
        /// <param name="subnet">The subnet in CIDR format, for example <c>172.28.0.0/16</c>.</param>
        /// <returns>This builder.</returns>
        public IpamConfigBuilder WithSubnet(string subnet)
        {
            Guard.Cidr(subnet, nameof(subnet));

            _config = _config with { Subnet = subnet };
            return this;
        }

        /// <summary>
        /// Sets the range of addresses from which container addresses are allocated.
        /// </summary>
        /// <param name="ipRange">The range in CIDR format, for example <c>172.28.5.0/24</c>.</param>
        /// <returns>This builder.</returns>
        public IpamConfigBuilder WithIpRange(string ipRange)
        {
            Guard.Cidr(ipRange, nameof(ipRange));

            _config = _config with { IpRange = ipRange };
            return this;
        }

        /// <summary>
        /// Sets the gateway for the subnet.
        /// </summary>
        /// <param name="gateway">The IPv4 or IPv6 gateway address.</param>
        /// <returns>This builder.</returns>
        public IpamConfigBuilder WithGateway(string gateway)
        {
            Guard.IpAddress(gateway, nameof(gateway));

            _config = _config with { Gateway = gateway };
            return this;
        }

        /// <summary>
        /// Sets an auxiliary address used by the network driver. Setting the same hostname again replaces it.
        /// </summary>
        /// <param name="host">The hostname.</param>
        /// <param name="address">The IPv4 or IPv6 address.</param>
        /// <returns>This builder.</returns>
        public IpamConfigBuilder WithAuxAddress(string host, string address)
        {
            Guard.NotNullOrWhiteSpace(host, nameof(host));
            Guard.IpAddress(address, nameof(address));

            _config = _config with { AuxAddresses = Collections.With(_config.AuxAddresses, host, address) };
            return this;
        }

        /// <summary>
        /// Sets several auxiliary addresses, as a mapping from hostname to address.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns>This builder.</returns>
        public IpamConfigBuilder WithAuxAddresses(IEnumerable<KeyValuePair<string, string>> addresses)
        {
            Guard.NotNull(addresses, nameof(addresses));

            foreach (KeyValuePair<string, string> address in addresses)
            {
                WithAuxAddress(address.Key, address.Value);
            }

            return this;
        }

        /// <summary>
        /// Creates the address pool from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="IpamConfigDefinition"/>.</returns>
        public IpamConfigDefinition Build()
        {
            return _config;
        }
    }
}
