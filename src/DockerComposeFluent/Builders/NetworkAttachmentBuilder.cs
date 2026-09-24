using System.Collections.Generic;
using System.Net.Sockets;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="NetworkAttachment"/>: the settings for one network that a service joins.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
    /// </summary>
    public sealed class NetworkAttachmentBuilder
    {
        private NetworkAttachment _attachment = new NetworkAttachment();

        /// <summary>
        /// Adds an alternative hostname for the service on this network. Each call adds another alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithAlias(string alias)
        {
            Guard.NotNullOrWhiteSpace(alias, nameof(alias));

            _attachment = _attachment with { Aliases = Collections.Append(_attachment.Aliases, alias) };
            return this;
        }

        /// <summary>
        /// Adds several alternative hostnames for the service on this network.
        /// </summary>
        /// <param name="aliases">The aliases.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithAliases(IEnumerable<string> aliases)
        {
            Guard.NotNull(aliases, nameof(aliases));

            foreach (string alias in aliases)
            {
                WithAlias(alias);
            }

            return this;
        }

        /// <summary>
        /// Sets a driver option. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithDriverOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _attachment = _attachment with { DriverOptions = Collections.With(_attachment.DriverOptions, key, value) };
            return this;
        }

        /// <summary>
        /// Sets several driver options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithDriverOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (KeyValuePair<string, string> option in options)
            {
                WithDriverOption(option.Key, option.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets the priority for selecting this network as the default gateway. Requires Compose 2.33.0 or later.
        /// </summary>
        /// <param name="gatewayPriority">The priority. The network with the highest value is used.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithGatewayPriority(int gatewayPriority)
        {
            _attachment = _attachment with { GatewayPriority = gatewayPriority };
            return this;
        }

        /// <summary>
        /// Sets the network interface name used to connect to this network. Requires Compose 2.36.0 or later.
        /// </summary>
        /// <param name="interfaceName">The interface name, for example <c>eth0</c>.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithInterfaceName(string interfaceName)
        {
            Guard.NotNullOrWhiteSpace(interfaceName, nameof(interfaceName));

            _attachment = _attachment with { InterfaceName = interfaceName };
            return this;
        }

        /// <summary>
        /// Sets a static IPv4 address for the container.
        /// </summary>
        /// <param name="ipv4Address">The address, for example <c>172.16.238.10</c>.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithIpv4Address(string ipv4Address)
        {
            Guard.IpAddress(ipv4Address, nameof(ipv4Address), AddressFamily.InterNetwork);

            _attachment = _attachment with { Ipv4Address = ipv4Address };
            return this;
        }

        /// <summary>
        /// Sets a static IPv6 address for the container.
        /// </summary>
        /// <param name="ipv6Address">The address, for example <c>2001:3984:3989::10</c>.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithIpv6Address(string ipv6Address)
        {
            Guard.IpAddress(ipv6Address, nameof(ipv6Address), AddressFamily.InterNetworkV6);

            _attachment = _attachment with { Ipv6Address = ipv6Address };
            return this;
        }

        /// <summary>
        /// Adds a link-local IP address. Each call adds another address.
        /// </summary>
        /// <param name="linkLocalIp">The IPv4 or IPv6 address.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithLinkLocalIp(string linkLocalIp)
        {
            Guard.IpAddress(linkLocalIp, nameof(linkLocalIp));

            _attachment = _attachment with { LinkLocalIps = Collections.Append(_attachment.LinkLocalIps, linkLocalIp) };
            return this;
        }

        /// <summary>
        /// Adds several link-local IP addresses.
        /// </summary>
        /// <param name="linkLocalIps">The IPv4 or IPv6 addresses.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithLinkLocalIps(IEnumerable<string> linkLocalIps)
        {
            Guard.NotNull(linkLocalIps, nameof(linkLocalIps));

            foreach (string linkLocalIp in linkLocalIps)
            {
                WithLinkLocalIp(linkLocalIp);
            }

            return this;
        }

        /// <summary>
        /// Sets the MAC address used when connecting to this network. Requires Compose 2.24.0 or later.
        /// </summary>
        /// <param name="macAddress">The MAC address.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithMacAddress(string macAddress)
        {
            Guard.NotNullOrWhiteSpace(macAddress, nameof(macAddress));

            _attachment = _attachment with { MacAddress = macAddress };
            return this;
        }

        /// <summary>
        /// Sets the order in which Compose connects the service's containers to its networks.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>This builder.</returns>
        public NetworkAttachmentBuilder WithPriority(int priority)
        {
            _attachment = _attachment with { Priority = priority };
            return this;
        }

        /// <summary>
        /// Creates the attachment from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="NetworkAttachment"/>.</returns>
        public NetworkAttachment Build()
        {
            return _attachment;
        }
    }
}
