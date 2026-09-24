using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="NetworkDefinition"/>.
    /// </summary>
    public sealed class NetworkBuilder
    {
        private NetworkDefinition _definition = new NetworkDefinition();

        /// <summary>
        /// Sets the network driver.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver"/>
        /// </summary>
        /// <param name="driver">The driver name.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _definition = _definition with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets the actual Docker network name, overriding the name generated from the project name and key.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#name"/>
        /// </summary>
        /// <param name="name">The network name.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithName(string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            _definition = _definition with { Name = name };
            return this;
        }

        /// <summary>
        /// Sets whether standalone containers can attach to the network as well as services.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#attachable"/>
        /// </summary>
        /// <param name="attachable"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithAttachable(bool attachable)
        {
            _definition = _definition with { Attachable = attachable };
            return this;
        }

        /// <summary>
        /// Sets a driver option. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver_opts"/>
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithDriverOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _definition = _definition with { DriverOptions = Collections.With(_definition.DriverOptions, key, value) };
            return this;
        }

        /// <summary>
        /// Sets several driver options.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver_opts"/>
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithDriverOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (KeyValuePair<string, string> option in options)
            {
                WithDriverOption(option.Key, option.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets whether IPv4 address assignment is enabled. Requires Compose 2.33.1 or later.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#enable_ipv4"/>
        /// </summary>
        /// <param name="enableIpv4"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithEnableIpv4(bool enableIpv4)
        {
            _definition = _definition with { EnableIpv4 = enableIpv4 };
            return this;
        }

        /// <summary>
        /// Sets whether IPv6 address assignment is enabled.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#enable_ipv6"/>
        /// </summary>
        /// <param name="enableIpv6"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithEnableIpv6(bool enableIpv6)
        {
            _definition = _definition with { EnableIpv6 = enableIpv6 };
            return this;
        }

        /// <summary>
        /// Sets whether the network's lifecycle is managed outside this application.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#external"/>
        /// </summary>
        /// <param name="external"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithExternal(bool external)
        {
            _definition = _definition with { External = external };
            return this;
        }

        /// <summary>
        /// Sets whether the network is isolated from the outside world.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#internal"/>
        /// </summary>
        /// <param name="internal"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithInternal(bool @internal)
        {
            _definition = _definition with { Internal = @internal };
            return this;
        }

        /// <summary>
        /// Sets a custom IP address management configuration from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
        /// </summary>
        /// <param name="ipam">The IPAM configuration.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithIpam(IpamDefinition ipam)
        {
            Guard.NotNull(ipam, nameof(ipam));

            _definition = _definition with { Ipam = ipam };
            return this;
        }

        /// <summary>
        /// Sets a custom IP address management configuration through an <see cref="IpamBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
        /// </summary>
        /// <param name="configure">Configures the IPAM settings.</param>
        /// <returns>This builder.</returns>
        public NetworkBuilder WithIpam(Action<IpamBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            IpamBuilder builder = new IpamBuilder();
            configure(builder);
            return WithIpam(builder.Build());
        }

        /// <summary>
        /// Creates the network definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="NetworkDefinition"/>.</returns>
        public NetworkDefinition Build()
        {
            return _definition;
        }
    }
}
