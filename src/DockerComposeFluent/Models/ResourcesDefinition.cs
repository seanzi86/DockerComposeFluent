namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The physical resource constraints and reservations for a service's containers (the
    /// <c>deploy.resources</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed record ResourcesDefinition
    {
        /// <summary>
        /// The upper bound on what a container may use, as specified by <c>limits</c>.
        /// </summary>
        public ResourceLimitsDefinition? Limits { get; init; }

        /// <summary>
        /// What the platform must guarantee a container can allocate, as specified by <c>reservations</c>.
        /// </summary>
        public ResourceReservationsDefinition? Reservations { get; init; }
    }
}
