using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A host device made available inside the container (an entry of <c>services.&lt;name&gt;.devices</c>).
    /// It is written as the short <c>source:target:permissions</c> string whenever that can express it, and as
    /// a mapping otherwise.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#devices"/>
    /// </summary>
    public sealed record DeviceMappingDefinition
    {
        /// <summary>
        /// The path of the device on the host, as specified by <c>source</c>.
        /// </summary>
        public string? Source { get; init; }

        /// <summary>
        /// The path the device is mapped to in the container, as specified by <c>target</c>. Defaults to the
        /// source path.
        /// </summary>
        public string? Target { get; init; }

        /// <summary>
        /// The cgroup permissions for the device, a combination of <c>r</c>, <c>w</c> and <c>m</c>, as
        /// specified by <c>permissions</c>. The default is <c>rwm</c>.
        /// </summary>
        public string? Permissions { get; init; }

        /// <summary>
        /// The extension fields defined on this {0}, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();

        /// <summary>
        /// Whether the entry can be written as a short string: it has no extension fields, and any
        /// permissions come with a target (the short syntax cannot skip the target).
        /// </summary>
        public bool CanBeShort
        {
            get { return Extensions.Count == 0 && (Permissions == null || Target != null); }
        }
    }
}
