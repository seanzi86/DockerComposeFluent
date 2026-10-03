namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The cgroup namespace a container joins, as specified by <c>cgroup</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cgroup"/>
    /// </summary>
    public enum CgroupMode
    {
        /// <summary>
        /// The container joins the host's cgroup namespace (<c>host</c>).
        /// </summary>
        Host,

        /// <summary>
        /// The container gets its own private cgroup namespace (<c>private</c>).
        /// </summary>
        Private
    }
}
