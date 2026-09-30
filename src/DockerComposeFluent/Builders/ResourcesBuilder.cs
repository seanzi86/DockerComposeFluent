using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ResourcesDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed class ResourcesBuilder
    {
        private ResourcesDefinition _definition = new ResourcesDefinition();

        /// <summary>
        /// Sets the upper bound on what a container may use, from an existing definition.
        /// </summary>
        /// <param name="limits">The resource limits.</param>
        /// <returns>This builder.</returns>
        public ResourcesBuilder WithLimits(ResourceLimitsDefinition limits)
        {
            Guard.NotNull(limits, nameof(limits));
            _definition = _definition with { Limits = limits };
            return this;
        }

        /// <summary>
        /// Sets the upper bound on what a container may use, configured through a
        /// <see cref="ResourceLimitsBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the limits.</param>
        /// <returns>This builder.</returns>
        public ResourcesBuilder WithLimits(Action<ResourceLimitsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ResourceLimitsBuilder builder = new ResourceLimitsBuilder();
            configure(builder);
            return WithLimits(builder.Build());
        }

        /// <summary>
        /// Sets what the platform must guarantee a container can allocate, from an existing definition.
        /// </summary>
        /// <param name="reservations">The resource reservations.</param>
        /// <returns>This builder.</returns>
        public ResourcesBuilder WithReservations(ResourceReservationsDefinition reservations)
        {
            Guard.NotNull(reservations, nameof(reservations));
            _definition = _definition with { Reservations = reservations };
            return this;
        }

        /// <summary>
        /// Sets what the platform must guarantee a container can allocate, configured through a
        /// <see cref="ResourceReservationsBuilder"/>.
        /// </summary>
        /// <param name="configure">Configures the reservations.</param>
        /// <returns>This builder.</returns>
        public ResourcesBuilder WithReservations(Action<ResourceReservationsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ResourceReservationsBuilder builder = new ResourceReservationsBuilder();
            configure(builder);
            return WithReservations(builder.Build());
        }

        /// <summary>
        /// Creates the resources definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ResourcesDefinition"/>.</returns>
        public ResourcesDefinition Build()
        {
            return _definition;
        }
    }
}
