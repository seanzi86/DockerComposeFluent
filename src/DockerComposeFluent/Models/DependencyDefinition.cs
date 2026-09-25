namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The settings for one dependency of a service, as specified under <c>services.&lt;name&gt;.depends_on</c>.
    /// A service whose dependencies all have no settings is written as a plain list of service names.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#depends_on"/>
    /// </summary>
    public sealed record DependencyDefinition
    {
        /// <summary>
        /// When the dependency is considered satisfied, as specified by <c>condition</c>. When it is not set but
        /// other settings are, <see cref="DependencyCondition.ServiceStarted"/> is written, because Compose requires
        /// the condition in the long syntax.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        public DependencyCondition? Condition { get; init; }

        /// <summary>
        /// Whether this service is restarted when the dependency is restarted by a Compose operation, as specified
        /// by <c>restart</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <remarks>Requires Compose 2.17.0 or later.</remarks>
        public bool? Restart { get; init; }

        /// <summary>
        /// Whether the dependency must be available. When <c>false</c>, Compose only warns if it is missing, as
        /// specified by <c>required</c>. Compose treats an unset value as <c>true</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <remarks>Requires Compose 2.20.0 or later.</remarks>
        public bool? Required { get; init; }

        /// <summary>
        /// Whether no setting is used, so the dependency is just a service name.
        /// </summary>
        public bool IsEmpty
        {
            get { return Condition == null && Restart == null && Required == null; }
        }
    }
}
