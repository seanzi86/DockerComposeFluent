namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The upper bound on the physical resources a container may use (the <c>deploy.resources.limits</c>
    /// entry). The platform must prevent the container allocating more.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#resources"/>
    /// </summary>
    public sealed record ResourceLimitsDefinition
    {
        /// <summary>
        /// The number of CPU cores the container can use, as specified by <c>cpus</c>, for example
        /// <c>"0.50"</c>.
        /// </summary>
        public string? Cpus { get; init; }

        /// <summary>
        /// The amount of memory the container can allocate, as a compose-spec byte value such as <c>50M</c>,
        /// as specified by <c>memory</c>.
        /// </summary>
        public string? Memory { get; init; }

        /// <summary>
        /// The container's PID limit, as specified by <c>pids</c>.
        /// </summary>
        public int? Pids { get; init; }
    }
}
