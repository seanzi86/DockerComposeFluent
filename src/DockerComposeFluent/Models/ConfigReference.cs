using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Grants a service access to one config (an entry of <c>services.&lt;name&gt;.configs</c>). An entry
    /// with only <see cref="Source"/> is written to YAML as a plain string; any other setting makes it a
    /// mapping.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
    /// </summary>
    public sealed record ConfigReference
    {
        /// <summary>
        /// The name of the config as defined in the top-level <c>configs</c> section, as specified by
        /// <c>source</c>.
        /// </summary>
        public string? Source { get; init; }

        /// <summary>
        /// The path and name of the file mounted in the container. Defaults to <c>/&lt;source&gt;</c> if not
        /// set, as specified by <c>target</c>.
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
        /// The extension fields defined on this reference, keyed by their <c>x-</c> name. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();

        /// <summary>
        /// Whether no setting other than <see cref="Source"/> is used, so the entry is just a config name.
        /// </summary>
        public bool IsSourceOnly
        {
            get { return Target == null && Uid == null && Gid == null && Mode == null && Extensions.Count == 0; }
        }
    }
}
