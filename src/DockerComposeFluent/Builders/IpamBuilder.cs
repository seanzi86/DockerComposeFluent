using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds an <see cref="IpamDefinition"/>: a custom IP address management configuration.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam"/>
    /// </summary>
    public sealed class IpamBuilder
    {
        private IpamDefinition _ipam = new IpamDefinition();

        /// <summary>
        /// Sets a custom IPAM driver.
        /// </summary>
        /// <param name="driver">The driver name.</param>
        /// <returns>This builder.</returns>
        public IpamBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));

            _ipam = _ipam with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Adds an address pool with just a subnet. Each call adds another pool.
        /// </summary>
        /// <param name="subnet">The subnet in CIDR format, for example <c>172.28.0.0/16</c>.</param>
        /// <returns>This builder.</returns>
        public IpamBuilder WithConfig(string subnet)
        {
            Guard.Cidr(subnet, nameof(subnet));

            return WithConfig(new IpamConfigDefinition { Subnet = subnet });
        }

        /// <summary>
        /// Adds an address pool from an existing definition. Each call adds another pool.
        /// </summary>
        /// <param name="config">The address pool.</param>
        /// <returns>This builder.</returns>
        public IpamBuilder WithConfig(IpamConfigDefinition config)
        {
            Guard.NotNull(config, nameof(config));

            _ipam = _ipam with { Config = Collections.Append(_ipam.Config, config) };
            return this;
        }

        /// <summary>
        /// Adds an address pool configured through an <see cref="IpamConfigBuilder"/>. Each call adds another pool.
        /// </summary>
        /// <param name="configure">Configures the address pool.</param>
        /// <returns>This builder.</returns>
        public IpamBuilder WithConfig(Action<IpamConfigBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            IpamConfigBuilder builder = new IpamConfigBuilder();
            configure(builder);
            return WithConfig(builder.Build());
        }

        /// <summary>
        /// Sets a driver-specific option. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public IpamBuilder WithOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _ipam = _ipam with { Options = Collections.With(_ipam.Options, key, value) };
            return this;
        }

        /// <summary>
        /// Sets several driver-specific options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>This builder.</returns>
        public IpamBuilder WithOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (KeyValuePair<string, string> option in options)
            {
                WithOption(option.Key, option.Value);
            }

            return this;
        }

        /// <summary>
        /// Creates the IPAM configuration from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="IpamDefinition"/>.</returns>
        public IpamDefinition Build()
        {
            return _ipam;
        }
    }
}
