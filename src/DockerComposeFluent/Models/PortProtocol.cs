namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The protocol a published port uses, as specified by <c>protocol</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
    /// </summary>
    public enum PortProtocol
    {
        /// <summary>
        /// Transmission Control Protocol (<c>tcp</c>), the default.
        /// </summary>
        Tcp,

        /// <summary>
        /// User Datagram Protocol (<c>udp</c>).
        /// </summary>
        Udp
    }
}
