using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Extra options for a named volume mount, as specified by <c>volume</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed record VolumeOptions
    {
        /// <summary>
        /// Whether to disable copying data from the container when the volume is created,
        /// as specified by <c>nocopy</c>.
        /// </summary>
        public bool? NoCopy { get; init; }

        /// <summary>
        /// A path inside the volume to mount instead of the volume root, as specified by <c>subpath</c>.
        /// </summary>
        public string? Subpath { get; init; }

        /// <summary>
        /// The extension fields defined on these volume options, keyed by their <c>x-</c> name. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
