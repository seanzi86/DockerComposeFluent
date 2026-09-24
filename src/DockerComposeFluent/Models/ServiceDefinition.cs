using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// Represents a single service (the <c>services.&lt;name&gt;</c> entry) in a Docker Compose file.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md"/>
    /// </summary>
    public sealed record ServiceDefinition
    {
        /// <summary>
        /// The image to start the container from, as specified by <c>image</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#image"/>
        /// </summary>
        public string? Image { get; init; }

        /// <summary>
        /// The command that overrides the image's default command, as specified by <c>command</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#command"/>
        /// </summary>
        public CommandLine? Command { get; init; }

        /// <summary>
        /// The custom container name, as specified by <c>container_name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#container_name"/>
        /// </summary>
        public string? ContainerName { get; init; }

        /// <summary>
        /// The entrypoint that overrides the image's default entrypoint, as specified by <c>entrypoint</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint"/>
        /// </summary>
        public CommandLine? Entrypoint { get; init; }

        /// <summary>
        /// The ports to expose, as specified by <c>ports</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        public IReadOnlyList<PortMapping> Ports { get; init; } = Array.Empty<PortMapping>();

        /// <summary>
        /// The mounts (volumes, bind mounts and so on), as specified by <c>volumes</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        public IReadOnlyList<MountMapping> Volumes { get; init; } = Array.Empty<MountMapping>();
    }
}
