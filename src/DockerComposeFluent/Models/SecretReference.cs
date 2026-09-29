namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Grants a service access to one secret (an entry of <c>services.&lt;name&gt;.secrets</c>). An entry
    /// with only <see cref="Source"/> is written to YAML as a plain string; any other setting makes it a
    /// mapping.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
    /// </summary>
    public sealed record SecretReference
    {
        /// <summary>
        /// The name of the secret as defined in the top-level <c>secrets</c> section, as specified by
        /// <c>source</c>.
        /// </summary>
        public string? Source { get; init; }

        /// <summary>
        /// The name of the file mounted under <c>/run/secrets/</c> in the container, or an absolute path for
        /// an alternate location. Defaults to <see cref="Source"/> if not set, as specified by <c>target</c>.
        /// </summary>
        public string? Target { get; init; }

        /// <summary>
        /// The numeric user ID that owns the mounted file, as specified by <c>uid</c>.
        /// </summary>
        public string? Uid { get; init; }

        /// <summary>
        /// The numeric group ID that owns the mounted file, as specified by <c>gid</c>.
        /// </summary>
        public string? Gid { get; init; }

        /// <summary>
        /// The permissions of the mounted file, in octal notation, as specified by <c>mode</c>. The default is
        /// world-readable (<c>0444</c>).
        /// </summary>
        public int? Mode { get; init; }

        /// <summary>
        /// Whether no setting other than <see cref="Source"/> is used, so the entry is just a secret name.
        /// </summary>
        public bool IsSourceOnly
        {
            get { return Target == null && Uid == null && Gid == null && Mode == null; }
        }
    }
}
