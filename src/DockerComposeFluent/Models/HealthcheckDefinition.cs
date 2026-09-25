namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The check that decides whether a service's containers are healthy (the <c>healthcheck</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
    /// </summary>
    public sealed record HealthcheckDefinition
    {
        /// <summary>
        /// Whether the healthcheck set by the image is disabled, as specified by <c>disable</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public bool? Disable { get; init; }

        /// <summary>
        /// The time between checks, as a compose-spec duration such as <c>1m30s</c>, as specified by
        /// <c>interval</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public string? Interval { get; init; }

        /// <summary>
        /// The number of consecutive failures before the container is considered unhealthy, as specified by
        /// <c>retries</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public int? Retries { get; init; }

        /// <summary>
        /// The time between checks during the start period, as a compose-spec duration, as specified by
        /// <c>start_interval</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <remarks>Requires Compose 2.20.2 or later.</remarks>
        public string? StartInterval { get; init; }

        /// <summary>
        /// How long the container has to start before failed checks count, as a compose-spec duration, as
        /// specified by <c>start_period</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public string? StartPeriod { get; init; }

        /// <summary>
        /// The command that checks the container's health, as specified by <c>test</c>: a shell-form string
        /// (which Compose runs with <c>CMD-SHELL</c>) or a list starting with <c>CMD</c>, <c>CMD-SHELL</c> or
        /// <c>NONE</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public CommandLine? Test { get; init; }

        /// <summary>
        /// How long a single check may run before it counts as failed, as a compose-spec duration, as specified
        /// by <c>timeout</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public string? Timeout { get; init; }
    }
}
