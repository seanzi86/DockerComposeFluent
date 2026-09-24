namespace DockerComposeFluent.Models
{
    /// <summary>
    /// A mount, given either as a short-syntax string (<c>"db-data:/data:ro"</c>) or as a long-syntax
    /// <see cref="MountDefinition"/>. The form chosen is preserved in the generated YAML.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
    /// </summary>
    public sealed record MountMapping
    {
        private MountMapping(string? shortSyntax, MountDefinition? definition)
        {
            ShortSyntax = shortSyntax;
            Definition = definition;
        }

        /// <summary>
        /// The mount as a short-syntax string, or <c>null</c> when it was given as a definition.
        /// </summary>
        public string? ShortSyntax { get; }

        /// <summary>
        /// The mount as a long-syntax definition, or <c>null</c> when it was given as a short-syntax string.
        /// </summary>
        public MountDefinition? Definition { get; }

        /// <summary>
        /// Creates a mount from a short-syntax string. The string is passed through as written.
        /// </summary>
        /// <param name="mount">The mount, for example <c>db-data:/var/lib/db</c> or <c>./src:/app:ro</c>.</param>
        /// <returns>A new <see cref="MountMapping"/>.</returns>
        public static MountMapping FromShortSyntax(string mount)
        {
            Guard.NotNullOrWhiteSpace(mount, nameof(mount));

            return new MountMapping(mount, null);
        }

        /// <summary>
        /// Creates a mount from a long-syntax definition.
        /// </summary>
        /// <param name="definition">The mount definition.</param>
        /// <returns>A new <see cref="MountMapping"/>.</returns>
        public static MountMapping FromDefinition(MountDefinition definition)
        {
            Guard.NotNull(definition, nameof(definition));

            return new MountMapping(null, definition);
        }
    }
}
