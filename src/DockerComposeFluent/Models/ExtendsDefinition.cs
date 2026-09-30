namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The other service a service extends, sharing its configuration as a base (the <c>extends</c> entry). An
    /// entry with only <see cref="Service"/> is written to YAML as a plain string; setting <see cref="File"/>
    /// makes it a mapping.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
    /// </summary>
    public sealed record ExtendsDefinition
    {
        /// <summary>
        /// The name of the service being referenced as a base, as specified by <c>service</c>.
        /// </summary>
        public string? Service { get; init; }

        /// <summary>
        /// The Compose file the referenced service is defined in, as specified by <c>file</c>. Relative paths
        /// are resolved against the main Compose file's location. Defaults to the current file, referencing
        /// another service within it.
        /// </summary>
        public string? File { get; init; }

        /// <summary>
        /// Whether no setting other than <see cref="Service"/> is used, so the entry is just a service name.
        /// </summary>
        public bool IsServiceOnly
        {
            get { return File == null; }
        }
    }
}
