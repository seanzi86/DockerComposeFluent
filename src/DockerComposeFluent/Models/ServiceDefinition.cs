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
        /// The services this service depends on and their settings, keyed by service name, as specified by
        /// <c>depends_on</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#depends_on"/>
        /// </summary>
        public IReadOnlyDictionary<string, DependencyDefinition> DependsOn { get; init; } = Collections.EmptyDictionary<DependencyDefinition>();

        /// <summary>
        /// Deployment metadata for the service, so a platform such as Docker Swarm can allocate and configure
        /// resources for it, as specified by <c>deploy</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md"/>
        /// </summary>
        public DeployDefinition? Deploy { get; init; }

        /// <summary>
        /// The entrypoint that overrides the image's default entrypoint, as specified by <c>entrypoint</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint"/>
        /// </summary>
        public CommandLine? Entrypoint { get; init; }

        /// <summary>
        /// The files to read environment variables from, as specified by <c>env_file</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
        /// </summary>
        public IReadOnlyList<EnvFileEntry> EnvFiles { get; init; } = Array.Empty<EnvFileEntry>();

        /// <summary>
        /// The environment variables set in the container, as specified by <c>environment</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        public EnvironmentVariables Environment { get; init; } = new EnvironmentVariables();

        /// <summary>
        /// The other service this service extends, sharing its configuration as a base, as specified by
        /// <c>extends</c>. Compose does not automatically pull in the referenced service's <c>volumes</c>,
        /// <c>networks</c>, <c>configs</c>, <c>secrets</c> or <c>depends_on</c> - declare them explicitly on
        /// this service if they are needed.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
        /// </summary>
        public ExtendsDefinition? Extends { get; init; }

        /// <summary>
        /// The labels attached to the service, as specified by <c>labels</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels"/>
        /// </summary>
        public Labels Labels { get; init; } = new Labels();

        /// <summary>
        /// The files to load labels from, as specified by <c>label_file</c>. Processed in order; a label in a
        /// later file overrides the same label from an earlier one. A label set directly in <see cref="Labels"/>
        /// takes precedence over one from these files. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#label_file"/>
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        public IReadOnlyList<string> LabelFiles { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The check that decides whether the service's containers are healthy, as specified by
        /// <c>healthcheck</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        public HealthcheckDefinition? Healthcheck { get; init; }

        /// <summary>
        /// The networks the service joins and their settings, keyed by network name, as specified by
        /// <c>networks</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        public IReadOnlyDictionary<string, NetworkAttachment> Networks { get; init; } = Collections.EmptyDictionary<NetworkAttachment>();

        /// <summary>
        /// The ports to expose, as specified by <c>ports</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        public IReadOnlyList<PortMapping> Ports { get; init; } = Array.Empty<PortMapping>();

        /// <summary>
        /// When the container is restarted, as specified by <c>restart</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        public RestartDefinition? Restart { get; init; }

        /// <summary>
        /// The logging configuration, as specified by <c>logging</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging"/>
        /// </summary>
        public LoggingDefinition? Logging { get; init; }

        /// <summary>
        /// The profiles the service is enabled under, as specified by <c>profiles</c>. A service with no profiles
        /// is always started; one with profiles only starts when one of them is activated. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#profiles"/>
        /// </summary>
        public IReadOnlyList<string> Profiles { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The configs this service is granted access to, as specified by <c>configs</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
        /// </summary>
        public IReadOnlyList<ConfigReference> Configs { get; init; } = Array.Empty<ConfigReference>();

        /// <summary>
        /// The secrets this service is granted access to, as specified by <c>secrets</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
        /// </summary>
        public IReadOnlyList<SecretReference> Secrets { get; init; } = Array.Empty<SecretReference>();

        /// <summary>
        /// The mounts (volumes, bind mounts and so on), as specified by <c>volumes</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        public IReadOnlyList<MountMapping> Volumes { get; init; } = Array.Empty<MountMapping>();

        /// <summary>
        /// The extension fields defined on this service, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();
    }
}
