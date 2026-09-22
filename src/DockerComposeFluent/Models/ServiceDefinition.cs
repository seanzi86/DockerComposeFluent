namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single service (the <c>services.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// </summary>
    public sealed record ServiceDefinition
    {
        /// <summary>
        /// The image to start the container from, as specified by <c>image</c>.
        /// </summary>
        public string? Image { get; init; }

        /// <summary>
        /// The custom container name, as specified by <c>container_name</c>.
        /// </summary>
        public string? ContainerName { get; init; }
    }
}
