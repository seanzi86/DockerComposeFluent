namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The propagation mode of a bind mount, as specified by <c>bind.propagation</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public enum BindPropagation
    {
        /// <summary>
        /// <c>private</c>.
        /// </summary>
        Private,

        /// <summary>
        /// <c>rprivate</c>.
        /// </summary>
        RPrivate,

        /// <summary>
        /// <c>shared</c>.
        /// </summary>
        Shared,

        /// <summary>
        /// <c>rshared</c>.
        /// </summary>
        RShared,

        /// <summary>
        /// <c>slave</c>.
        /// </summary>
        Slave,

        /// <summary>
        /// <c>rslave</c>.
        /// </summary>
        RSlave
    }
}
