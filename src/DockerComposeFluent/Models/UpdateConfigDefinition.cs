namespace DockerComposeFluent.Models
{
    /// <summary>
    /// How to roll out an update to a service, such as for a rolling update (the <c>deploy.update_config</c>
    /// entry).
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#update_config"/>
    /// </summary>
    public sealed record UpdateConfigDefinition
    {
        /// <summary>
        /// The number of containers to update at a time, as specified by <c>parallelism</c>.
        /// </summary>
        public int? Parallelism { get; init; }

        /// <summary>
        /// The time to wait between updating each group of containers, as a compose-spec duration, as
        /// specified by <c>delay</c>.
        /// </summary>
        public string? Delay { get; init; }

        /// <summary>
        /// What to do if the update fails, as specified by <c>failure_action</c>.
        /// </summary>
        public UpdateFailureAction? FailureAction { get; init; }

        /// <summary>
        /// How long to monitor each task for failure after it is updated, as a compose-spec duration, as
        /// specified by <c>monitor</c>.
        /// </summary>
        public string? Monitor { get; init; }

        /// <summary>
        /// The failure rate to tolerate during the update, from <c>0</c> to <c>1</c>, as specified by
        /// <c>max_failure_ratio</c>.
        /// </summary>
        public double? MaxFailureRatio { get; init; }

        /// <summary>
        /// The order containers are stopped and started in, as specified by <c>order</c>.
        /// </summary>
        public RolloutOrder? Order { get; init; }
    }
}
