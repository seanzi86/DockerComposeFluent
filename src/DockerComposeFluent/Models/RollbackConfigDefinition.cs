using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// How to roll a service back after a failed update (the <c>deploy.rollback_config</c> entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#rollback_config"/>
    /// </summary>
    public sealed record RollbackConfigDefinition
    {
        /// <summary>
        /// The number of containers to roll back at a time, as specified by <c>parallelism</c>. <c>0</c> rolls
        /// every container back at once.
        /// </summary>
        public int? Parallelism { get; init; }

        /// <summary>
        /// The time to wait between each group's rollback, as a compose-spec duration, as specified by
        /// <c>delay</c>.
        /// </summary>
        public string? Delay { get; init; }

        /// <summary>
        /// What to do if the rollback itself fails, as specified by <c>failure_action</c>.
        /// </summary>
        public RollbackFailureAction? FailureAction { get; init; }

        /// <summary>
        /// How long to monitor each task for failure after it is updated, as a compose-spec duration, as
        /// specified by <c>monitor</c>.
        /// </summary>
        public string? Monitor { get; init; }

        /// <summary>
        /// The failure rate to tolerate during the rollback, from <c>0</c> to <c>1</c>, as specified by
        /// <c>max_failure_ratio</c>.
        /// </summary>
        public double? MaxFailureRatio { get; init; }

        /// <summary>
        /// The order containers are stopped and started in, as specified by <c>order</c>.
        /// </summary>
        public RolloutOrder? Order { get; init; }

        /// <summary>
        /// The extension fields defined on <c>deploy.rollback_config</c>, keyed by their <c>x-</c> name.
        /// Compose ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
