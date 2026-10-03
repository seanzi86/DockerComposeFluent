using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ResourceReservationsDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed class ResourceReservationsBuilder
    {
        private ResourceReservationsDefinition _definition = new ResourceReservationsDefinition();

        /// <summary>
        /// Sets the number of CPU cores the container can use.
        /// </summary>
        /// <param name="cpus">The core count, for example <c>"0.25"</c>.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithCpus(string cpus)
        {
            Guard.NotNullOrWhiteSpace(cpus, nameof(cpus));
            _definition = _definition with { Cpus = cpus };
            return this;
        }

        /// <summary>
        /// Sets the amount of memory the container can allocate.
        /// </summary>
        /// <param name="memory">A compose-spec byte value, such as <c>20M</c>.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithMemory(string memory)
        {
            Guard.NotNullOrWhiteSpace(memory, nameof(memory));
            _definition = _definition with { Memory = memory };
            return this;
        }

        /// <summary>
        /// Sets the container's PID limit.
        /// </summary>
        /// <param name="pids">The limit, which must be positive.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithPids(int pids)
        {
            if (pids < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pids), pids, "The PID limit must be positive.");
            }

            _definition = _definition with { Pids = pids };
            return this;
        }

        /// <summary>
        /// Adds a device to reserve, from an existing definition.
        /// </summary>
        /// <param name="device">The device definition.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithDevice(DeviceDefinition device)
        {
            Guard.NotNull(device, nameof(device));
            _definition = _definition with { Devices = Collections.Append(_definition.Devices, device) };
            return this;
        }

        /// <summary>
        /// Adds a device to reserve, configured through a <see cref="DeviceBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the device.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithDevice(Action<DeviceBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            DeviceBuilder builder = new DeviceBuilder();
            configure(builder);
            return WithDevice(builder.Build());
        }

        /// <summary>
        /// Adds a countable user-defined resource to reserve.
        /// </summary>
        /// <param name="kind">The kind of resource, for example <c>GPU</c> or <c>SSD</c>.</param>
        /// <param name="value">The number to reserve, which must be positive.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithGenericResource(string kind, int value)
        {
            return WithGenericResource(new GenericResourceBuilder().WithDiscreteResourceSpec(kind, value).Build());
        }

        /// <summary>
        /// Adds a user-defined resource to reserve, from an existing definition.
        /// </summary>
        /// <param name="resource">The generic resource.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithGenericResource(GenericResourceDefinition resource)
        {
            Guard.NotNull(resource, nameof(resource));
            _definition = _definition with { GenericResources = Collections.Append(_definition.GenericResources, resource) };
            return this;
        }

        /// <summary>
        /// Adds a user-defined resource to reserve, configured through a <see cref="GenericResourceBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the generic resource.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithGenericResource(Action<GenericResourceBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            GenericResourceBuilder builder = new GenericResourceBuilder();
            configure(builder);
            return WithGenericResource(builder.Build());
        }

        /// <summary>
        /// Sets an extension field on <c>deploy.resources.reservations</c>. Setting the same key again
        /// replaces its value. Compose ignores these; they exist for the file's own reuse (YAML anchors) or
        /// for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public ResourceReservationsBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the resource reservations from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ResourceReservationsDefinition"/>.</returns>
        public ResourceReservationsDefinition Build()
        {
            return _definition;
        }
    }
}
