namespace DockerComposeFluent.Models
{
    /// <summary>
    /// One file of environment variables for a service, as specified under <c>env_file</c>. An entry with only a
    /// <see cref="Path"/> is written to YAML as a plain string; any other setting makes it a mapping.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
    /// </summary>
    public sealed record EnvFileEntry
    {
        /// <summary>
        /// The file path, resolved relative to the Compose file's folder.
        /// </summary>
        public string? Path { get; init; }

        /// <summary>
        /// The alternative parsing format for the file, as specified by <c>format</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#format"/>
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public EnvFileFormat? Format { get; init; }

        /// <summary>
        /// Whether the file must exist. When <c>false</c>, Compose silently ignores a missing file. Compose
        /// treats an unset value as <c>true</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#required"/>
        /// </summary>
        /// <remarks>Requires Compose 2.24.0 or later.</remarks>
        public bool? Required { get; init; }

        /// <summary>
        /// Whether no setting other than the path is used, so the entry is just a file path.
        /// </summary>
        public bool IsPathOnly
        {
            get { return Format == null && Required == null; }
        }
    }
}
