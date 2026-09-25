using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The settings for one network that a service joins, as specified under <c>services.&lt;name&gt;.networks</c>.
    /// A service whose networks all have no settings is written as a plain list of network names.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
    /// </summary>
    public sealed record NetworkAttachment
    {
        /// <summary>
        /// Alternative hostnames for the service on this network, as specified by <c>aliases</c>.
        /// </summary>
        public IReadOnlyList<string> Aliases { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>driver_opts</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.27.1 or later.</remarks>
        public IReadOnlyDictionary<string, string> DriverOptions { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The priority for selecting this network as the default gateway, as specified by <c>gw_priority</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.33.0 or later.</remarks>
        public int? GatewayPriority { get; init; }

        /// <summary>
        /// The network interface name used to connect to this network, as specified by <c>interface_name</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.36.0 or later.</remarks>
        public string? InterfaceName { get; init; }

        /// <summary>
        /// A static IPv4 address for the container, as specified by <c>ipv4_address</c>.
        /// </summary>
        public string? Ipv4Address { get; init; }

        /// <summary>
        /// A static IPv6 address for the container, as specified by <c>ipv6_address</c>.
        /// </summary>
        public string? Ipv6Address { get; init; }

        /// <summary>
        /// Link-local IP addresses, as specified by <c>link_local_ips</c>.
        /// </summary>
        public IReadOnlyList<string> LinkLocalIps { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The MAC address used when connecting to this network, as specified by <c>mac_address</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.24.0 or later.</remarks>
        public string? MacAddress { get; init; }

        /// <summary>
        /// The order in which Compose connects the service's containers to its networks, as specified by
        /// <c>priority</c>.
        /// </summary>
        public int? Priority { get; init; }

        /// <summary>
        /// Whether no setting is used, so the attachment is just a network name.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Aliases.Count == 0
                    && DriverOptions.Count == 0
                    && GatewayPriority == null
                    && InterfaceName == null
                    && Ipv4Address == null
                    && Ipv6Address == null
                    && LinkLocalIps.Count == 0
                    && MacAddress == null
                    && Priority == null;
            }
        }
    }
}
