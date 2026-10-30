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
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#image"/>
        /// </summary>
        public string? Image { get; init; }

        /// <summary>
        /// The command that overrides the image's default command, as specified by <c>command</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#command"/>
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
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/workload_spec.md#depends_on"/>
        /// </summary>
        public IReadOnlyDictionary<string, DependencyDefinition> DependsOn
        {
            get { return _dependsOn; }
            init { _dependsOn = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, DependencyDefinition> _dependsOn = Collections.EmptyDictionary<DependencyDefinition>();

        /// <summary>
        /// Deployment metadata for the service, so a platform such as Docker Swarm can allocate and configure
        /// resources for it, as specified by <c>deploy</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md"/>
        /// </summary>
        public DeployDefinition? Deploy { get; init; }

        /// <summary>
        /// The entrypoint that overrides the image's default entrypoint, as specified by <c>entrypoint</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#entrypoint"/>
        /// </summary>
        public CommandLine? Entrypoint { get; init; }

        /// <summary>
        /// The files to read environment variables from, as specified by <c>env_file</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#env_file"/>
        /// </summary>
        public IReadOnlyList<EnvFileEntry> EnvFiles
        {
            get { return _envFiles; }
            init { _envFiles = Collections.Seal(value); }
        }

        private IReadOnlyList<EnvFileEntry> _envFiles = Collections.EmptyList<EnvFileEntry>();

        /// <summary>
        /// The environment variables set in the container, as specified by <c>environment</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#environment"/>
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
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#labels"/>
        /// </summary>
        public Labels Labels { get; init; } = new Labels();

        /// <summary>
        /// The files to load labels from, as specified by <c>label_file</c>. Processed in order; a label in a
        /// later file overrides the same label from an earlier one. A label set directly in <see cref="Labels"/>
        /// takes precedence over one from these files. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#label_file"/>
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        public IReadOnlyList<string> LabelFiles
        {
            get { return _labelFiles; }
            init { _labelFiles = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _labelFiles = Collections.EmptyList<string>();

        /// <summary>
        /// The check that decides whether the service's containers are healthy, as specified by
        /// <c>healthcheck</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/workload_spec.md#healthcheck"/>
        /// </summary>
        public HealthcheckDefinition? Healthcheck { get; init; }

        /// <summary>
        /// The networks the service joins and their settings, keyed by network name, as specified by
        /// <c>networks</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#networks"/>
        /// </summary>
        public IReadOnlyDictionary<string, NetworkAttachment> Networks
        {
            get { return _networks; }
            init { _networks = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, NetworkAttachment> _networks = Collections.EmptyDictionary<NetworkAttachment>();

        /// <summary>
        /// The target platform to run the container on, as specified by <c>platform</c>, for example
        /// <c>linux/amd64</c> or <c>linux/arm64</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#platform"/>
        /// </summary>
        public string? Platform { get; init; }

        /// <summary>
        /// The ports to expose, as specified by <c>ports</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/workload_spec.md#ports"/>
        /// </summary>
        public IReadOnlyList<PortMapping> Ports
        {
            get { return _ports; }
            init { _ports = Collections.Seal(value); }
        }

        private IReadOnlyList<PortMapping> _ports = Collections.EmptyList<PortMapping>();

        /// <summary>
        /// When the container is restarted, as specified by <c>restart</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        public RestartDefinition? Restart { get; init; }

        /// <summary>
        /// The logging configuration, as specified by <c>logging</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#logging"/>
        /// </summary>
        public LoggingDefinition? Logging { get; init; }

        /// <summary>
        /// The profiles the service is enabled under, as specified by <c>profiles</c>. A service with no profiles
        /// is always started; one with profiles only starts when one of them is activated. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#profiles"/>
        /// </summary>
        public IReadOnlyList<string> Profiles
        {
            get { return _profiles; }
            init { _profiles = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _profiles = Collections.EmptyList<string>();

        /// <summary>
        /// The configs this service is granted access to, as specified by <c>configs</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#configs"/>
        /// </summary>
        public IReadOnlyList<ConfigReference> Configs
        {
            get { return _configs; }
            init { _configs = Collections.Seal(value); }
        }

        private IReadOnlyList<ConfigReference> _configs = Collections.EmptyList<ConfigReference>();

        /// <summary>
        /// The secrets this service is granted access to, as specified by <c>secrets</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#secrets"/>
        /// </summary>
        public IReadOnlyList<SecretReference> Secrets
        {
            get { return _secrets; }
            init { _secrets = Collections.Seal(value); }
        }

        private IReadOnlyList<SecretReference> _secrets = Collections.EmptyList<SecretReference>();

        /// <summary>
        /// The mounts (volumes, bind mounts and so on), as specified by <c>volumes</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#volumes"/>
        /// </summary>
        public IReadOnlyList<MountMapping> Volumes
        {
            get { return _volumes; }
            init { _volumes = Collections.Seal(value); }
        }

        private IReadOnlyList<MountMapping> _volumes = Collections.EmptyList<MountMapping>();

        /// <summary>
        /// Whether Compose collects the service's logs when attached (<c>docker compose up</c>), as specified by <c>attach</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#attach"/>
        /// </summary>
        /// <remarks>Requires Compose 2.20.0 or later.</remarks>
        public bool? Attach { get; init; }

        /// <summary>
        /// The custom domain name for the container, as specified by <c>domainname</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#domainname"/>
        /// </summary>
        public string? DomainName { get; init; }

        /// <summary>
        /// The custom hostname for the container, as specified by <c>hostname</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#hostname"/>
        /// </summary>
        public string? Hostname { get; init; }

        /// <summary>
        /// Whether to run an init process inside the container that forwards signals and reaps processes, as specified by <c>init</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#init"/>
        /// </summary>
        public bool? Init { get; init; }

        /// <summary>
        /// The container isolation technology; supported values are platform-specific, as specified by <c>isolation</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#isolation"/>
        /// </summary>
        public string? Isolation { get; init; }

        /// <summary>
        /// The MAC address of the container, as specified by <c>mac_address</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#mac_address"/>
        /// </summary>
        public string? MacAddress { get; init; }

        /// <summary>
        /// Whether the container has extended privileges, as specified by <c>privileged</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#privileged"/>
        /// </summary>
        public bool? Privileged { get; init; }

        /// <summary>
        /// When to pull the image: <c>always</c>, <c>never</c>, <c>missing</c>, <c>if_not_present</c>, <c>build</c>, <c>refresh</c>, <c>daily</c>, <c>weekly</c> or <c>every_&lt;duration&gt;</c>, as specified by <c>pull_policy</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#pull_policy"/>
        /// </summary>
        /// <remarks>The <c>refresh</c>, <c>daily</c>, <c>weekly</c> and <c>every_&lt;duration&gt;</c> policies were added in Compose 2.34.0.</remarks>
        public string? PullPolicy { get; init; }

        /// <summary>
        /// How long after which to refresh the image, as a compose-spec duration, used with a <c>refresh</c> pull policy, as specified by <c>pull_refresh_after</c>. The compose-spec text does not describe this key; it appears only in the compose-go schema, from the same Compose release as the <c>refresh</c> pull policy.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#pull_policy"/>
        /// </summary>
        /// <remarks>Requires Compose 2.34.0 or later.</remarks>
        public string? PullRefreshAfter { get; init; }

        /// <summary>
        /// Whether the container's filesystem is mounted read-only, as specified by <c>read_only</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#read_only"/>
        /// </summary>
        public bool? ReadOnly { get; init; }

        /// <summary>
        /// The runtime to use for the container, for example <c>runc</c>, as specified by <c>runtime</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#runtime"/>
        /// </summary>
        public string? Runtime { get; init; }

        /// <summary>
        /// The number of containers to run for the service, as specified by <c>scale</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#scale"/>
        /// </summary>
        public int? Scale { get; init; }

        /// <summary>
        /// Whether to keep STDIN open even if not attached, as specified by <c>stdin_open</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/workload_spec.md#stdin_open"/>
        /// </summary>
        public bool? StdinOpen { get; init; }

        /// <summary>
        /// How long to wait for the container to stop gracefully before sending SIGKILL, as a compose-spec duration, as specified by <c>stop_grace_period</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#stop_grace_period"/>
        /// </summary>
        public string? StopGracePeriod { get; init; }

        /// <summary>
        /// The signal used to stop the container, for example <c>SIGTERM</c>, as specified by <c>stop_signal</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#stop_signal"/>
        /// </summary>
        public string? StopSignal { get; init; }

        /// <summary>
        /// Whether to allocate a pseudo-TTY, as specified by <c>tty</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/workload_spec.md#tty"/>
        /// </summary>
        public bool? Tty { get; init; }

        /// <summary>
        /// Whether to bind mount the Docker API socket and its required auth into the container, as specified by <c>use_api_socket</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#use_api_socket"/>
        /// </summary>
        /// <remarks>Requires Compose 2.37.2 or later.</remarks>
        public bool? UseApiSocket { get; init; }

        /// <summary>
        /// The username or UID to run the container process as, as specified by <c>user</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#user"/>
        /// </summary>
        public string? User { get; init; }

        /// <summary>
        /// The working directory the entrypoint or command runs in, as specified by <c>working_dir</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#working_dir"/>
        /// </summary>
        public string? WorkingDir { get; init; }

        /// <summary>
        /// The network mode of the container, for example <c>host</c>, <c>none</c> or <c>service:&lt;name&gt;</c>, as specified by <c>network_mode</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#network_mode"/>
        /// </summary>
        public string? NetworkMode { get; init; }

        /// <summary>
        /// The IPC isolation mode, for example <c>shareable</c> or <c>service:&lt;name&gt;</c>, as specified by <c>ipc</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#ipc"/>
        /// </summary>
        public string? Ipc { get; init; }

        /// <summary>
        /// The PID namespace mode, for example <c>host</c> or <c>service:&lt;name&gt;</c>, as specified by <c>pid</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#pid"/>
        /// </summary>
        public string? Pid { get; init; }

        /// <summary>
        /// The UTS namespace mode, for example <c>host</c>, as specified by <c>uts</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#uts"/>
        /// </summary>
        /// <remarks>Requires Compose 2.15.1 or later.</remarks>
        public string? Uts { get; init; }

        /// <summary>
        /// The user namespace mode, for example <c>host</c>, as specified by <c>userns_mode</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#userns_mode"/>
        /// </summary>
        public string? UsernsMode { get; init; }

        /// <summary>
        /// The cgroup namespace to join, as specified by <c>cgroup</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cgroup"/>
        /// </summary>
        /// <remarks>Requires Compose 2.16.0 or later.</remarks>
        public CgroupMode? Cgroup { get; init; }

        /// <summary>
        /// The parent cgroup for the container, as specified by <c>cgroup_parent</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cgroup_parent"/>
        /// </summary>
        public string? CgroupParent { get; init; }

        /// <summary>
        /// The Linux capabilities to add, for example <c>NET_ADMIN</c>, as specified by <c>cap_add</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cap_add"/>
        /// </summary>
        public IReadOnlyList<string> CapAdd
        {
            get { return _capAdd; }
            init { _capAdd = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _capAdd = Collections.EmptyList<string>();

        /// <summary>
        /// The Linux capabilities to drop, for example <c>ALL</c>, as specified by <c>cap_drop</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cap_drop"/>
        /// </summary>
        public IReadOnlyList<string> CapDrop
        {
            get { return _capDrop; }
            init { _capDrop = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _capDrop = Collections.EmptyList<string>();

        /// <summary>
        /// The security options for the container, for example <c>no-new-privileges:true</c>, as specified by <c>security_opt</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#security_opt"/>
        /// </summary>
        public IReadOnlyList<string> SecurityOpt
        {
            get { return _securityOpt; }
            init { _securityOpt = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _securityOpt = Collections.EmptyList<string>();

        /// <summary>
        /// The additional groups (names or numeric IDs) the container user joins, as specified by <c>group_add</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#group_add"/>
        /// </summary>
        public IReadOnlyList<string> GroupAdd
        {
            get { return _groupAdd; }
            init { _groupAdd = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _groupAdd = Collections.EmptyList<string>();

        /// <summary>
        /// The device cgroup rules, for example <c>c 1:3 mr</c>, as specified by <c>device_cgroup_rules</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#device_cgroup_rules"/>
        /// </summary>
        public IReadOnlyList<string> DeviceCgroupRules
        {
            get { return _deviceCgroupRules; }
            init { _deviceCgroupRules = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _deviceCgroupRules = Collections.EmptyList<string>();

        /// <summary>
        /// The custom DNS servers, as specified by <c>dns</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#dns"/>
        /// </summary>
        public IReadOnlyList<string> Dns
        {
            get { return _dns; }
            init { _dns = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _dns = Collections.EmptyList<string>();

        /// <summary>
        /// The custom DNS options, as specified by <c>dns_opt</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#dns_opt"/>
        /// </summary>
        public IReadOnlyList<string> DnsOpt
        {
            get { return _dnsOpt; }
            init { _dnsOpt = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _dnsOpt = Collections.EmptyList<string>();

        /// <summary>
        /// The custom DNS search domains, as specified by <c>dns_search</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#dns_search"/>
        /// </summary>
        public IReadOnlyList<string> DnsSearch
        {
            get { return _dnsSearch; }
            init { _dnsSearch = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _dnsSearch = Collections.EmptyList<string>();

        /// <summary>
        /// The ports exposed to linked services without publishing them to the host, for example <c>3000</c> or <c>8000-8010</c>, as specified by <c>expose</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/workload_spec.md#expose"/>
        /// </summary>
        public IReadOnlyList<string> Expose
        {
            get { return _expose; }
            init { _expose = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _expose = Collections.EmptyList<string>();

        /// <summary>
        /// The links to other services, as <c>service</c> or <c>service:alias</c>, as specified by <c>links</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#links"/>
        /// </summary>
        public IReadOnlyList<string> Links
        {
            get { return _links; }
            init { _links = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _links = Collections.EmptyList<string>();

        /// <summary>
        /// The links to containers started outside this file, as <c>container</c> or <c>container:alias</c>, as specified by <c>external_links</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#external_links"/>
        /// </summary>
        public IReadOnlyList<string> ExternalLinks
        {
            get { return _externalLinks; }
            init { _externalLinks = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _externalLinks = Collections.EmptyList<string>();

        /// <summary>
        /// The services or containers to mount all volumes from, as <c>service</c>, <c>service:ro</c> or <c>container:name</c>, as specified by <c>volumes_from</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#volumes_from"/>
        /// </summary>
        public IReadOnlyList<string> VolumesFrom
        {
            get { return _volumesFrom; }
            init { _volumesFrom = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _volumesFrom = Collections.EmptyList<string>();

        /// <summary>
        /// The temporary filesystems to mount, as paths with optional options, as specified by <c>tmpfs</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#tmpfs"/>
        /// </summary>
        public IReadOnlyList<string> Tmpfs
        {
            get { return _tmpfs; }
            init { _tmpfs = Collections.Seal(value); }
        }

        private IReadOnlyList<string> _tmpfs = Collections.EmptyList<string>();

        /// <summary>
        /// The number of CPUs the container can use, for example <c>0.5</c>, as specified by <c>cpus</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpus"/>
        /// </summary>
        public string? Cpus { get; init; }

        /// <summary>
        /// The number of usable CPUs (Windows), as specified by <c>cpu_count</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_count"/>
        /// </summary>
        public int? CpuCount { get; init; }

        /// <summary>
        /// The percentage of usable CPU (Windows), from 0 to 100, as specified by <c>cpu_percent</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_percent"/>
        /// </summary>
        public int? CpuPercent { get; init; }

        /// <summary>
        /// The relative CPU weight, as specified by <c>cpu_shares</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_shares"/>
        /// </summary>
        public long? CpuShares { get; init; }

        /// <summary>
        /// The CFS CPU quota in microseconds, as specified by <c>cpu_quota</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_quota"/>
        /// </summary>
        public long? CpuQuota { get; init; }

        /// <summary>
        /// The CFS CPU period in microseconds, as specified by <c>cpu_period</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_period"/>
        /// </summary>
        public long? CpuPeriod { get; init; }

        /// <summary>
        /// The CPU real-time period in microseconds, as specified by <c>cpu_rt_period</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_rt_period"/>
        /// </summary>
        public long? CpuRtPeriod { get; init; }

        /// <summary>
        /// The CPU real-time runtime in microseconds, as specified by <c>cpu_rt_runtime</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpu_rt_runtime"/>
        /// </summary>
        public long? CpuRtRuntime { get; init; }

        /// <summary>
        /// The CPUs the container may run on, for example <c>0-3</c> or <c>0,1</c>, as specified by <c>cpuset</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#cpuset"/>
        /// </summary>
        public string? Cpuset { get; init; }

        /// <summary>
        /// The memory limit, as a compose-spec byte value such as <c>512m</c>, as specified by <c>mem_limit</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#mem_limit"/>
        /// </summary>
        public string? MemLimit { get; init; }

        /// <summary>
        /// The soft memory limit, as a compose-spec byte value such as <c>256m</c>, as specified by <c>mem_reservation</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#mem_reservation"/>
        /// </summary>
        public string? MemReservation { get; init; }

        /// <summary>
        /// The memory plus swap limit, as a compose-spec byte value; <c>-1</c> is unlimited swap, as specified by <c>memswap_limit</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#memswap_limit"/>
        /// </summary>
        public string? MemswapLimit { get; init; }

        /// <summary>
        /// How readily the kernel swaps container memory, from 0 to 100, as specified by <c>mem_swappiness</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#mem_swappiness"/>
        /// </summary>
        public int? MemSwappiness { get; init; }

        /// <summary>
        /// Whether to disable the out-of-memory killer for the container, as specified by <c>oom_kill_disable</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#oom_kill_disable"/>
        /// </summary>
        public bool? OomKillDisable { get; init; }

        /// <summary>
        /// The out-of-memory preference, from -1000 to 1000, as specified by <c>oom_score_adj</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#oom_score_adj"/>
        /// </summary>
        public int? OomScoreAdj { get; init; }

        /// <summary>
        /// The maximum number of processes, or <c>-1</c> for unlimited, as specified by <c>pids_limit</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#pids_limit"/>
        /// </summary>
        public int? PidsLimit { get; init; }

        /// <summary>
        /// The size of <c>/dev/shm</c>, as a compose-spec byte value such as <c>64m</c>, as specified by <c>shm_size</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#shm_size"/>
        /// </summary>
        public string? ShmSize { get; init; }

        /// <summary>
        /// The block I/O configuration, as specified by <c>blkio_config</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#blkio_config"/>
        /// </summary>
        public BlkioConfigDefinition? BlkioConfig { get; init; }

        /// <summary>
        /// The credential spec for a managed service account, as specified by <c>credential_spec</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#credential_spec"/>
        /// </summary>
        public CredentialSpecDefinition? CredentialSpec { get; init; }

        /// <summary>
        /// The host devices made available in the container, as specified by <c>devices</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#devices"/>
        /// </summary>
        public IReadOnlyList<DeviceMappingDefinition> Devices
        {
            get { return _devices; }
            init { _devices = Collections.Seal(value); }
        }

        private IReadOnlyList<DeviceMappingDefinition> _devices = Collections.EmptyList<DeviceMappingDefinition>();

        /// <summary>
        /// The additional hostnames added to the container's <c>/etc/hosts</c>, keyed by hostname with the address as the value, as specified by <c>extra_hosts</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#extra_hosts"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> ExtraHosts
        {
            get { return _extraHosts; }
            init { _extraHosts = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, string> _extraHosts = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The GPUs the container may use, as specified by <c>gpus</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#gpus"/>
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public GpusDefinition? Gpus { get; init; }

        /// <summary>
        /// The storage driver options for the container, as specified by <c>storage_opt</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#storage_opt"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> StorageOpt
        {
            get { return _storageOpt; }
            init { _storageOpt = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, string> _storageOpt = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The kernel parameters to set in the container, as specified by <c>sysctls</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#sysctls"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> Sysctls
        {
            get { return _sysctls; }
            init { _sysctls = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, string> _sysctls = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The resource limits for processes in the container, keyed by limit name such as <c>nofile</c>, as specified by <c>ulimits</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#ulimits"/>
        /// </summary>
        public IReadOnlyDictionary<string, UlimitDefinition> Ulimits
        {
            get { return _ulimits; }
            init { _ulimits = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, UlimitDefinition> _ulimits = Collections.EmptyDictionary<UlimitDefinition>();

        /// <summary>
        /// The annotations attached to the container, as specified by <c>annotations</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/container_spec.md#annotations"/>
        /// </summary>
        /// <remarks>Requires Compose 2.18.0 or later.</remarks>
        public IReadOnlyDictionary<string, string> Annotations
        {
            get { return _annotations; }
            init { _annotations = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, string> _annotations = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The commands run after the container starts, as specified by <c>post_start</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#post_start"/>
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public IReadOnlyList<ServiceHookDefinition> PostStart
        {
            get { return _postStart; }
            init { _postStart = Collections.Seal(value); }
        }

        private IReadOnlyList<ServiceHookDefinition> _postStart = Collections.EmptyList<ServiceHookDefinition>();

        /// <summary>
        /// The commands run before the container stops, as specified by <c>pre_stop</c>. Empty when none are
        /// set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_stop"/>
        /// </summary>
        /// <remarks>Requires Compose 2.30.0 or later.</remarks>
        public IReadOnlyList<ServiceHookDefinition> PreStop
        {
            get { return _preStop; }
            init { _preStop = Collections.Seal(value); }
        }

        private IReadOnlyList<ServiceHookDefinition> _preStop = Collections.EmptyList<ServiceHookDefinition>();

        /// <summary>
        /// How to build the service's image from source, as specified by <c>build</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md"/>
        /// </summary>
        public BuildDefinition? Build { get; init; }

        /// <summary>
        /// The settings that help while developing the service, such as rules that react to file changes, as
        /// specified by <c>develop</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/develop.md"/>
        /// </summary>
        /// <remarks>Requires Compose 2.22.0 or later.</remarks>
        public DevelopDefinition? Develop { get; init; }

        /// <summary>
        /// The init-container steps run to completion before the service starts, in order, as specified by
        /// <c>pre_start</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_start"/>
        /// </summary>
        /// <remarks>Requires Compose 5.3.0 or later.</remarks>
        public IReadOnlyList<PreStartDefinition> PreStart
        {
            get { return _preStart; }
            init { _preStart = Collections.Seal(value); }
        }

        private IReadOnlyList<PreStartDefinition> _preStart = Collections.EmptyList<PreStartDefinition>();

        /// <summary>
        /// The extension fields defined on this service, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions
        {
            get { return _extensions; }
            init { _extensions = Collections.Seal(value); }
        }

        private IReadOnlyDictionary<string, object?> _extensions = Collections.EmptyDictionary<object?>();
    }
}
