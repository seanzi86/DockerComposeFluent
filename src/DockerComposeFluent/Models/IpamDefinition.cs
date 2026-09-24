using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A custom IP address management (IPAM) configuration for a network, as specified by <c>ipam</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
    /// </summary>
    public sealed record IpamDefinition
    {
        /// <summary>
        /// The address pools, as specified by <c>config</c>.
        /// </summary>
        public IReadOnlyList<IpamConfigDefinition> Config { get; init; } = Array.Empty<IpamConfigDefinition>();

        /// <summary>
        /// A custom IPAM driver, as specified by <c>driver</c>.
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>options</c>.
        /// </summary>
        public IReadOnlyDictionary<string, string> Options { get; init; } = Collections.EmptyDictionary<string>();
    }
}
