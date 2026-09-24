namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The kind of mount, as specified by <c>type</c> in the long syntax.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public enum MountType
    {
        /// <summary>
        /// A named volume (<c>volume</c>).
        /// </summary>
        Volume,

        /// <summary>
        /// A host path (<c>bind</c>).
        /// </summary>
        Bind,

        /// <summary>
        /// A temporary in-memory filesystem (<c>tmpfs</c>).
        /// </summary>
        Tmpfs,

        /// <summary>
        /// The contents of an image (<c>image</c>).
        /// </summary>
        Image,

        /// <summary>
        /// A Windows named pipe (<c>npipe</c>).
        /// </summary>
        Npipe,

        /// <summary>
        /// A cluster volume (<c>cluster</c>).
        /// </summary>
        Cluster
    }
}
