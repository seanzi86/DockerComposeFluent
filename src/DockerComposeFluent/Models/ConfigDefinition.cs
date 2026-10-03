using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single config (the <c>configs.&lt;name&gt;</c> entry) in a Docker Compose file:
    /// configuration data granted to services that explicitly request it via
    /// <c>services.&lt;name&gt;.configs</c>. Exactly one of <see cref="File"/>, <see cref="Environment"/> or
    /// <see cref="Content"/> is set, or <see cref="External"/> is <c>true</c> with none of the others set.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
    /// </summary>
    public sealed record ConfigDefinition
    {
        /// <summary>
        /// The path of the file whose contents become the config, as specified by <c>file</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        public string? File { get; init; }

        /// <summary>
        /// The name of the environment variable whose value becomes the config, as specified by
        /// <c>environment</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        public string? Environment { get; init; }

        /// <summary>
        /// The inline text that becomes the config, as specified by <c>content</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        /// <remarks>Requires Compose 2.23.1 or later.</remarks>
        public string? Content { get; init; }

        /// <summary>
        /// Whether this config has already been created outside Compose, as specified by <c>external</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        public bool? External { get; init; }

        /// <summary>
        /// The actual name of the config object to look up, overriding the default name generated from the
        /// project name and the key this config is defined under, as specified by <c>name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/08-configs.md"/>
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The extension fields defined on this config, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
