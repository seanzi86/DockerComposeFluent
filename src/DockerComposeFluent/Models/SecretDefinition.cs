namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single secret (the <c>secrets.&lt;name&gt;</c> entry) in a Docker Compose file: sensitive
    /// data granted to services that explicitly request it via <c>services.&lt;name&gt;.secrets</c>. Exactly
    /// one of <see cref="File"/> or <see cref="Environment"/> is set, or <see cref="External"/> is
    /// <c>true</c> with none of the others set.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
    /// </summary>
    public sealed record SecretDefinition
    {
        /// <summary>
        /// The path of the file whose contents become the secret, as specified by <c>file</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public string? File { get; init; }

        /// <summary>
        /// The name of the environment variable whose value becomes the secret, as specified by
        /// <c>environment</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        /// <remarks>Requires Compose 2.6.0 or later.</remarks>
        public string? Environment { get; init; }

        /// <summary>
        /// Whether this secret has already been created outside Compose, as specified by <c>external</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public bool? External { get; init; }

        /// <summary>
        /// The actual name of the secret object to look up, overriding the default name generated from the
        /// project name and the key this secret is defined under, as specified by <c>name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public string? Name { get; init; }
    }
}
