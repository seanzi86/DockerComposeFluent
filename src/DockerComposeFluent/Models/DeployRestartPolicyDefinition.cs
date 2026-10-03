using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// How and whether to restart a service's containers when they exit (the <c>deploy.restart_policy</c>
    /// entry). Distinct from the top-level <c>restart</c> shorthand (<see cref="RestartDefinition"/>), which
    /// Compose falls back to when this is not set.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#restart_policy"/>
    /// </summary>
    public sealed record DeployRestartPolicyDefinition
    {
        /// <summary>
        /// When a container is restarted, as specified by <c>condition</c>.
        /// </summary>
        public DeployRestartCondition? Condition { get; init; }

        /// <summary>
        /// How long to wait between restart attempts, as a compose-spec duration, as specified by
        /// <c>delay</c>. The default is immediate.
        /// </summary>
        public string? Delay { get; init; }

        /// <summary>
        /// The maximum number of restart attempts before giving up, as specified by <c>max_attempts</c>. The
        /// default is to never give up.
        /// </summary>
        public int? MaxAttempts { get; init; }

        /// <summary>
        /// How long to wait before deciding a restart has succeeded, as a compose-spec duration, as specified
        /// by <c>window</c>. The default is to decide immediately.
        /// </summary>
        public string? Window { get; init; }

        /// <summary>
        /// The extension fields defined on <c>deploy.restart_policy</c>, keyed by their <c>x-</c> name.
        /// Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
