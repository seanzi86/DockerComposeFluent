namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The SELinux re-labelling option of a bind mount, as specified by <c>bind.selinux</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public enum SelinuxLabel
    {
        /// <summary>
        /// The content is shared among multiple containers (<c>z</c>).
        /// </summary>
        Shared,

        /// <summary>
        /// The content is private and unshared (<c>Z</c>).
        /// </summary>
        Private
    }
}
