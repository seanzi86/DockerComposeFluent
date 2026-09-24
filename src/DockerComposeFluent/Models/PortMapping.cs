namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A port mapping, given either as a short-syntax string (<c>"127.0.0.1:8080:80/tcp"</c>) or as a
    /// long-syntax <see cref="PortDefinition"/>. The form chosen is preserved in the generated YAML.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
    /// </summary>
    public sealed record PortMapping
    {
        private PortMapping(string? shortSyntax, PortDefinition? definition)
        {
            ShortSyntax = shortSyntax;
            Definition = definition;
        }

        /// <summary>
        /// The mapping as a short-syntax string, or <c>null</c> when it was given as a definition.
        /// </summary>
        public string? ShortSyntax { get; }

        /// <summary>
        /// The mapping as a long-syntax definition, or <c>null</c> when it was given as a short-syntax string.
        /// </summary>
        public PortDefinition? Definition { get; }

        /// <summary>
        /// Creates a mapping from a short-syntax string. The string is passed through as written.
        /// </summary>
        /// <param name="port">The mapping, for example <c>8080:80</c> or <c>127.0.0.1:8080:80/udp</c>.</param>
        /// <returns>A new <see cref="PortMapping"/>.</returns>
        public static PortMapping FromShortSyntax(string port)
        {
            Guard.NotNullOrWhiteSpace(port, nameof(port));

            return new PortMapping(port, null);
        }

        /// <summary>
        /// Creates a mapping from a long-syntax definition.
        /// </summary>
        /// <param name="definition">The port definition.</param>
        /// <returns>A new <see cref="PortMapping"/>.</returns>
        public static PortMapping FromDefinition(PortDefinition definition)
        {
            Guard.NotNull(definition, nameof(definition));

            return new PortMapping(null, definition);
        }
    }
}
