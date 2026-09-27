using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The logging configuration for a service, as specified by <c>logging</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging"/>
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
        public IReadOnlyDictionary<string, string> Options { get; init; } = Collections.EmptyDictionary<string>();
    }
}
