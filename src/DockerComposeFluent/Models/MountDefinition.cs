namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A mount in long syntax, with every field the compose-spec supports.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed record MountDefinition
    {
        /// <summary>
        /// Creates a mount definition.
        /// </summary>
        /// <param name="type">The kind of mount.</param>
        /// <param name="target">The path in the container where the mount appears.</param>
        public MountDefinition(MountType type, string target)
        {
            Guard.NotNullOrWhiteSpace(target, nameof(target));

            Type = type;
            Target = target;
        }

        /// <summary>
        /// The kind of mount, as specified by <c>type</c>.
        /// </summary>
        public MountType Type { get; }

        /// <summary>
        /// The path in the container where the mount appears, as specified by <c>target</c>.
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// Extra options for a bind mount, as specified by <c>bind</c>.
        /// </summary>
        public BindOptions? Bind { get; init; }

        /// <summary>
        /// The consistency requirements of the mount, as specified by <c>consistency</c>. The available
        /// values are platform specific.
        /// </summary>
        public string? Consistency { get; init; }

        /// <summary>
        /// Extra options for an image mount, as specified by <c>image</c>.
        /// </summary>
        public ImageOptions? Image { get; init; }

        /// <summary>
        /// Whether the mount is read-only, as specified by <c>read_only</c>.
        /// </summary>
        public bool? ReadOnly { get; init; }

        /// <summary>
        /// The source of the mount, as specified by <c>source</c>: a host path for a bind mount, a volume
        /// name for a volume, or an image reference for an image mount. Not applicable to tmpfs.
        /// </summary>
        public string? Source { get; init; }

        /// <summary>
        /// Extra options for a tmpfs mount, as specified by <c>tmpfs</c>.
        /// </summary>
        public TmpfsOptions? Tmpfs { get; init; }

        /// <summary>
        /// Extra options for a named volume mount, as specified by <c>volume</c>.
        /// </summary>
        public VolumeOptions? Volume { get; init; }
    }
}
