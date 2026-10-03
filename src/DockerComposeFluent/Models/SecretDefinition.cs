using System.Collections.Generic;

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

        /// <summary>
        /// The driver that provides this secret, as specified by <c>driver</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>driver_opts</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> DriverOptions { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The labels attached to the secret, as specified by <c>labels</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public Labels Labels { get; init; } = new Labels();

        /// <summary>
        /// The driver used to template the secret's value, as specified by <c>template_driver</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md"/>
        /// </summary>
        public string? TemplateDriver { get; init; }

        /// <summary>
        /// The extension fields defined on this secret, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
