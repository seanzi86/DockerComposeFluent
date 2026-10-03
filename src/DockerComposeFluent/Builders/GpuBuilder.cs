using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="GpuDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#gpus"/>
    /// </summary>
    public sealed class GpuBuilder
    {
        private GpuDefinition _definition = new GpuDefinition();

        /// <summary>
        /// Adds a capability the GPU must have.
        /// </summary>
        /// <param name="capability">The capability, for example <c>compute</c> or <c>utility</c>.</param>
        /// <returns>This builder.</returns>
        public GpuBuilder WithCapability(string capability)
        {
            Guard.NotNullOrWhiteSpace(capability, nameof(capability));
            _definition = _definition with { Capabilities = Collections.Append(_definition.Capabilities, capability) };
            return this;
        }

        /// <summary>
        /// Sets the number of GPUs to use. Mutually exclusive with <see cref="WithDeviceId(string)"/>.
        /// </summary>
        /// <param name="count">The number of GPUs, which must be positive.</param>
        /// <returns>This builder.</returns>
        public GpuBuilder WithCount(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "The GPU count must be positive.");
            }

            _definition = _definition with { Count = count };
            return this;
        }

        /// <summary>
        /// Adds the ID of a specific GPU to use. Mutually exclusive with <see cref="WithCount(int)"/>.
        /// </summary>
        /// <param name="deviceId">The device ID.</param>
        /// <returns>This builder.</returns>
        public GpuBuilder WithDeviceId(string deviceId)
        {
            Guard.NotNullOrWhiteSpace(deviceId, nameof(deviceId));
            _definition = _definition with { DeviceIds = Collections.Append(_definition.DeviceIds, deviceId) };
            return this;
        }

        /// <summary>
        /// Sets the GPU driver to use.
        /// </summary>
        /// <param name="driver">The driver name, for example <c>nvidia</c>.</param>
        /// <returns>This builder.</returns>
        public GpuBuilder WithDriver(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            _definition = _definition with { Driver = driver };
            return this;
        }

        /// <summary>
        /// Sets a driver-specific option. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public GpuBuilder WithOption(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));

            _definition = _definition with { Options = Collections.With(_definition.Options, key, value) };
            return this;
        }

        /// <summary>
        /// Creates the GPU request from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="GpuDefinition"/>.</returns>
        /// <exception cref="InvalidOperationException">Both a count and device IDs were set.</exception>
        public GpuDefinition Build()
        {
            if (_definition.Count != null && _definition.DeviceIds.Count > 0)
            {
                throw new InvalidOperationException("WithCount and WithDeviceId are mutually exclusive.");
            }

            return _definition;
        }
    }
}
