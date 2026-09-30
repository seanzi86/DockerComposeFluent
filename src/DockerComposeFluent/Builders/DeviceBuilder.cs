using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="DeviceDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#devices"/>
    /// </summary>
    public sealed class DeviceBuilder
    {
        private DeviceDefinition _definition = new DeviceDefinition();

        /// <summary>
        /// Adds a capability the device must satisfy.
        /// </summary>
        /// <param name="capability">The capability, for example <c>gpu</c> or a driver-specific value such as
        /// <c>nvidia-compute</c>.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithCapability(string capability)
        {
            Guard.NotNullOrWhiteSpace(capability, nameof(capability));
            _definition = _definition with { Capabilities = Collections.Append(_definition.Capabilities, capability) };
            return this;
        }

        /// <summary>
        /// Adds several capabilities the device must satisfy.
        /// </summary>
        /// <param name="capabilities">The capabilities.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithCapabilities(IEnumerable<string> capabilities)
        {
            Guard.NotNull(capabilities, nameof(capabilities));

            foreach (string capability in capabilities)
            {
                WithCapability(capability);
            }

            return this;
        }

        /// <summary>
        /// Sets the driver to reserve the device through.
        /// </summary>
        /// <param name="driver">The driver name, for example <c>nvidia</c>.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _definition = _definition with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets the number of devices to reserve. Mutually exclusive with <see cref="WithDeviceId(string)"/>.
        /// </summary>
        /// <param name="count">The number of devices, which must be positive.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithCount(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "The device count must be positive.");
            }

            _definition = _definition with { Count = count };
            return this;
        }

        /// <summary>
        /// Adds the ID of a specific device to reserve. Mutually exclusive with <see cref="WithCount(int)"/>.
        /// </summary>
        /// <param name="deviceId">The device ID.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithDeviceId(string deviceId)
        {
            Guard.NotNullOrWhiteSpace(deviceId, nameof(deviceId));
            _definition = _definition with { DeviceIds = Collections.Append(_definition.DeviceIds, deviceId) };
            return this;
        }

        /// <summary>
        /// Adds the IDs of several specific devices to reserve. Mutually exclusive with
        /// <see cref="WithCount(int)"/>.
        /// </summary>
        /// <param name="deviceIds">The device IDs.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithDeviceIds(IEnumerable<string> deviceIds)
        {
            Guard.NotNull(deviceIds, nameof(deviceIds));

            foreach (string deviceId in deviceIds)
            {
                WithDeviceId(deviceId);
            }

            return this;
        }

        /// <summary>
        /// Sets a driver-specific option. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public DeviceBuilder WithOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _definition = _definition with { Options = Collections.With(_definition.Options, key, value) };
            return this;
        }

        /// <summary>
        /// Creates the device definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="DeviceDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">No capability was set, or both
        /// <see cref="WithCount(int)"/> and a device ID were set.</exception>
        public DeviceDefinition Build()
        {
            if (_definition.Capabilities.Count == 0)
            {
                throw new InvalidOperationException("A device reservation must set at least one capability.");
            }

            if (_definition.Count != null && _definition.DeviceIds.Count > 0)
            {
                throw new InvalidOperationException("WithCount and WithDeviceId/WithDeviceIds are mutually exclusive.");
            }

            return _definition;
        }
    }
}
