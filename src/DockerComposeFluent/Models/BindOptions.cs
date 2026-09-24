namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Extra options for a bind mount, as specified by <c>bind</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed record BindOptions
    {
        /// <summary>
        /// Whether to create a directory at the source path on the host if it does not exist,
        /// as specified by <c>create_host_path</c>.
        /// </summary>
        public bool? CreateHostPath { get; init; }

        /// <summary>
        /// The propagation mode used for the bind, as specified by <c>propagation</c>.
        /// </summary>
        public BindPropagation? Propagation { get; init; }

        /// <summary>
        /// The SELinux re-labelling option, as specified by <c>selinux</c>.
        /// </summary>
        public SelinuxLabel? Selinux { get; init; }
    }
}
