using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Another Compose application or sub-project to include (an entry of the top-level <c>include</c> list).
    /// An entry with only a single <see cref="Path"/> and nothing else is written to YAML as a plain string;
    /// any other setting, or more than one path, makes it a mapping.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/14-include.md"/>
    /// </summary>
    public sealed record IncludeDefinition
    {
        /// <summary>
        /// The paths to the Compose files to include, as specified by <c>path</c>. Never empty.
        /// </summary>
        public IReadOnlyList<string> Path { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The paths to environment files that provide default values when interpolating variables in the
        /// included files, as specified by <c>env_file</c>. Empty when none are set.
        /// </summary>
        public IReadOnlyList<string> EnvFile { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The directory relative paths in the included file are resolved against, as specified by
        /// <c>project_directory</c>. Defaults to the included file's own directory if not set.
        /// </summary>
        public string? ProjectDirectory { get; init; }

        /// <summary>
        /// Whether no setting other than a single <see cref="Path"/> is used, so the entry is just a path.
        /// </summary>
        public bool IsPathOnly
        {
            get { return Path.Count == 1 && EnvFile.Count == 0 && ProjectDirectory == null; }
        }
    }
}
