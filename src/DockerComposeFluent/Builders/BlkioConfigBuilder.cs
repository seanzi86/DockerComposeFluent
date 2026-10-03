using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="BlkioConfigDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
    /// </summary>
    public sealed class BlkioConfigBuilder
    {
        private BlkioConfigDefinition _definition = new BlkioConfigDefinition();

        /// <summary>
        /// Sets the relative block I/O weight of the service.
        /// </summary>
        /// <param name="weight">The weight, from 10 to 1000.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithWeight(int weight)
        {
            if (weight < 10 || weight > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(weight), weight, "The weight must be between 10 and 1000.");
            }

            _definition = _definition with { Weight = weight };
            return this;
        }

        /// <summary>
        /// Adds a block I/O weight for a specific device.
        /// </summary>
        /// <param name="path">The device path, for example <c>/dev/sda</c>.</param>
        /// <param name="weight">The weight, from 10 to 1000.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithWeightDevice(string path, int weight)
        {
            return WithWeightDevice(new BlkioWeightBuilder().WithPath(path).WithWeight(weight).Build());
        }

        /// <summary>
        /// Adds a block I/O weight for a specific device, from an existing definition.
        /// </summary>
        /// <param name="weight">The weight definition.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithWeightDevice(BlkioWeightDefinition weight)
        {
            Guard.NotNull(weight, nameof(weight));
            _definition = _definition with { WeightDevice = Collections.Append(_definition.WeightDevice, weight) };
            return this;
        }

        /// <summary>
        /// Adds a block I/O weight for a specific device, configured through a <see cref="BlkioWeightBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the weight.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithWeightDevice(Action<BlkioWeightBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            BlkioWeightBuilder builder = new BlkioWeightBuilder();
            configure(builder);
            return WithWeightDevice(builder.Build());
        }

        /// <summary>
        /// Adds a read limit in bytes per second for a device.
        /// </summary>
        /// <param name="path">The device path.</param>
        /// <param name="rate">The rate, for example <c>12mb</c>.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceReadBps(string path, string rate)
        {
            return WithDeviceReadBps(new BlkioLimitBuilder().WithPath(path).WithRate(rate).Build());
        }

        /// <summary>
        /// Adds a read limit in bytes per second for a device, from an existing definition.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceReadBps(BlkioLimitDefinition limit)
        {
            Guard.NotNull(limit, nameof(limit));
            _definition = _definition with { DeviceReadBps = Collections.Append(_definition.DeviceReadBps, limit) };
            return this;
        }

        /// <summary>
        /// Adds a read limit in operations per second for a device.
        /// </summary>
        /// <param name="path">The device path.</param>
        /// <param name="rate">The operations per second, for example <c>120</c>.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceReadIops(string path, string rate)
        {
            return WithDeviceReadIops(new BlkioLimitBuilder().WithPath(path).WithRate(rate).Build());
        }

        /// <summary>
        /// Adds a read limit in operations per second for a device, from an existing definition.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceReadIops(BlkioLimitDefinition limit)
        {
            Guard.NotNull(limit, nameof(limit));
            _definition = _definition with { DeviceReadIops = Collections.Append(_definition.DeviceReadIops, limit) };
            return this;
        }

        /// <summary>
        /// Adds a write limit in bytes per second for a device.
        /// </summary>
        /// <param name="path">The device path.</param>
        /// <param name="rate">The rate, for example <c>1gb</c>.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceWriteBps(string path, string rate)
        {
            return WithDeviceWriteBps(new BlkioLimitBuilder().WithPath(path).WithRate(rate).Build());
        }

        /// <summary>
        /// Adds a write limit in bytes per second for a device, from an existing definition.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceWriteBps(BlkioLimitDefinition limit)
        {
            Guard.NotNull(limit, nameof(limit));
            _definition = _definition with { DeviceWriteBps = Collections.Append(_definition.DeviceWriteBps, limit) };
            return this;
        }

        /// <summary>
        /// Adds a write limit in operations per second for a device.
        /// </summary>
        /// <param name="path">The device path.</param>
        /// <param name="rate">The operations per second, for example <c>30</c>.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceWriteIops(string path, string rate)
        {
            return WithDeviceWriteIops(new BlkioLimitBuilder().WithPath(path).WithRate(rate).Build());
        }

        /// <summary>
        /// Adds a write limit in operations per second for a device, from an existing definition.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>This builder.</returns>
        public BlkioConfigBuilder WithDeviceWriteIops(BlkioLimitDefinition limit)
        {
            Guard.NotNull(limit, nameof(limit));
            _definition = _definition with { DeviceWriteIops = Collections.Append(_definition.DeviceWriteIops, limit) };
            return this;
        }

        /// <summary>
        /// Creates the block I/O configuration from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="BlkioConfigDefinition"/>.</returns>
        public BlkioConfigDefinition Build()
        {
            return _definition;
        }
    }
}
