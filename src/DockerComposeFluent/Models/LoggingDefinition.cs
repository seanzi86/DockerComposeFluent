using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The logging configuration for a service, as specified by <c>logging</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#logging"/>
    /// </summary>
    public sealed record LoggingDefinition
    {
        /// <summary>
        /// The logging driver, as specified by <c>driver</c>. The default and the available drivers are
        /// platform-specific.
        /// </summary>
        public string? Driver { get; init; }

        /// <summary>
        /// Driver-specific options as key-value pairs, as specified by <c>options</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyDictionary<string, string> Options
        {
            get { return _options; }
            init { _options = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, string> _options = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The extension fields defined on this logging configuration, keyed by their <c>x-</c> name. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions
        {
            get { return _extensions; }
            init { _extensions = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, object?> _extensions = Collections.EmptyDictionary<object?>();
    }
}
