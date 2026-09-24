using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// One address pool of an IPAM configuration, as specified by <c>ipam.config</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
    /// </summary>
    public sealed record IpamConfigDefinition
    {
        /// <summary>
        /// Auxiliary addresses used by the network driver, as a mapping from hostname to IP address,
        /// as specified by <c>aux_addresses</c>.
        /// </summary>
        public IReadOnlyDictionary<string, string> AuxAddresses { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The IPv4 or IPv6 gateway for the subnet, as specified by <c>gateway</c>.
        /// </summary>
        public string? Gateway { get; init; }

        /// <summary>
        /// The range of addresses from which container addresses are allocated, in CIDR format,
        /// as specified by <c>ip_range</c>.
        /// </summary>
        public string? IpRange { get; init; }

        /// <summary>
        /// The subnet in CIDR format, as specified by <c>subnet</c>.
        /// </summary>
        public string? Subnet { get; init; }
    }
}
