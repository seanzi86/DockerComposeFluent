using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single network (the <c>networks.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md"/>
    /// </summary>
    public sealed record NetworkDefinition
    {
        /// <summary>
        /// Whether standalone containers can attach to the network as well as services, as specified by
        /// <c>attachable</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#attachable"/>
        /// </summary>
        public bool? Attachable { get; init; }

        /// <summary>
        /// The network driver to use, as specified by <c>driver</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver"/>
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>driver_opts</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver_opts"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> DriverOptions { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// Whether IPv4 address assignment is enabled, as specified by <c>enable_ipv4</c>.
        /// Requires Compose 2.33.1 or later.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#enable_ipv4"/>
        /// </summary>
        public bool? EnableIpv4 { get; init; }

        /// <summary>
        /// Whether IPv6 address assignment is enabled, as specified by <c>enable_ipv6</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#enable_ipv6"/>
        /// </summary>
        public bool? EnableIpv6 { get; init; }

        /// <summary>
        /// Whether the network's lifecycle is managed outside this application, as specified by <c>external</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#external"/>
        /// </summary>
        public bool? External { get; init; }

        /// <summary>
        /// Whether the network is isolated from the outside world, as specified by <c>internal</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#internal"/>
        /// </summary>
        public bool? Internal { get; init; }

        /// <summary>
        /// A custom IP address management configuration, as specified by <c>ipam</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
        /// </summary>
        public IpamDefinition? Ipam { get; init; }

        /// <summary>
        /// The actual Docker network name to use, overriding the default name generated from the
        /// project name and the key this network is defined under, as specified by <c>name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#name"/>
        /// </summary>
        public string? Name { get; init; }
    }
}
