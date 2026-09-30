namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The service discovery method for external clients connecting to a service, as specified by
    /// <c>endpoint_mode</c> under <c>deploy</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md#endpoint_mode"/>
    /// </summary>
    public enum EndpointMode
    {
        /// <summary>
        /// A virtual IP fronts the service, and the platform routes requests between clients and nodes
        /// (<c>vip</c>), the default.
        /// </summary>
        Vip,

        /// <summary>
        /// DNS round-robin: a DNS query for the service name returns every node's address, and clients connect
        /// directly (<c>dnsrr</c>).
        /// </summary>
        Dnsrr
    }
}
