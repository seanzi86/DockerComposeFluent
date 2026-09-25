namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A port mapping in long syntax, with every field the compose-spec supports.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
    /// </summary>
    public sealed record PortDefinition
    {
        /// <summary>
        /// Creates a port definition for the given container port.
        /// </summary>
        /// <param name="target">The container port, from 1 to 65535.</param>
        public PortDefinition(int target)
        {
            Guard.ValidPort(target, nameof(target));

            Target = target;
        }

        /// <summary>
        /// The container port, as specified by <c>target</c>.
        /// </summary>
        public int Target { get; }

        /// <summary>
        /// The application protocol this port is used for, such as <c>http</c>, as specified by <c>app_protocol</c>.
        /// </summary>
        /// <remarks>Requires Compose 2.26.0 or later.</remarks>
        public string? AppProtocol { get; init; }

        /// <summary>
        /// The host IP address to bind to, as specified by <c>host_ip</c>. When not set, all interfaces are used.
        /// </summary>
        public string? HostIp { get; init; }

        /// <summary>
        /// How the port is published, as specified by <c>mode</c>.
        /// </summary>
        public PortMode? Mode { get; init; }

        /// <summary>
        /// A human-readable name documenting the port's use, as specified by <c>name</c>.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The port protocol, as specified by <c>protocol</c>.
        /// </summary>
        public PortProtocol? Protocol { get; init; }

        /// <summary>
        /// The publicly exposed port, or a range such as <c>8000-9000</c>, as specified by <c>published</c>.
        /// </summary>
        public string? Published { get; init; }
    }
}
