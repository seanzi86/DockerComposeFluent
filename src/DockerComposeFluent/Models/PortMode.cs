namespace DockerComposeFluent.Models
{
    /// <summary>
    /// How a port is published, as specified by <c>mode</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
    /// </summary>
    public enum PortMode
    {
        /// <summary>
        /// Publish a host port on each node (<c>host</c>).
        /// </summary>
        Host,

        /// <summary>
        /// Load balance the port (<c>ingress</c>), the default.
        /// </summary>
        Ingress
    }
}
