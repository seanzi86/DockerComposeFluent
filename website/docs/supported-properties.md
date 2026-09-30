---
sidebar_position: 4
---

# Supported properties

Every property below has a `WithX` builder method that produces it; see the [API reference](./api-reference)
for the exact overloads. "Since Compose" is the minimum Docker Compose version the property needs, taken from
the library's own `<remarks>` on that property; "Core spec" means there's no extra requirement.

<!-- supported-properties:begin -->

### Include (`include`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `path` | The paths to the Compose files to include. | Core spec |
| `env_file` | The paths to environment files that provide default values when interpolating variables in the included files. | Core spec |
| `project_directory` | The directory relative paths in the included file are resolved against. | Core spec |

### Services (`services.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`image`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#image) | The image to start the container from. | Core spec |
| [`command`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#command) | The command that overrides the image's default command. | Core spec |
| [`container_name`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#container_name) | The custom container name. | Core spec |
| [`depends_on`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#depends_on) | The services this service depends on and their settings, keyed by service name. | Core spec |
| [`deploy`](https://github.com/compose-spec/compose-spec/blob/main/deploy.md) | Deployment metadata for the service, so a platform such as Docker Swarm can allocate and configure resources for it. | Core spec |
| [`entrypoint`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint) | The entrypoint that overrides the image's default entrypoint. | Core spec |
| [`env_file`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file) | The files to read environment variables from. | Core spec |
| [`environment`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment) | The environment variables set in the container. | Core spec |
| [`extends`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends) | The other service this service extends, sharing its configuration as a base. | Core spec |
| [`labels`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels) | The labels attached to the service. | Core spec |
| [`label_file`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#label_file) | The files to load labels from. | 2.32.0+ |
| [`healthcheck`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | The check that decides whether the service's containers are healthy. | Core spec |
| [`networks`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks) | The networks the service joins and their settings, keyed by network name. | Core spec |
| [`platform`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#platform) | The target platform to run the container on. | Core spec |
| [`ports`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports) | The ports to expose. | Core spec |
| [`restart`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart) | When the container is restarted. | Core spec |
| [`logging`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging) | The logging configuration. | Core spec |
| [`profiles`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#profiles) | The profiles the service is enabled under. | Core spec |
| [`configs`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs) | The configs this service is granted access to. | Core spec |
| [`secrets`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets) | The secrets this service is granted access to. | Core spec |
| [`volumes`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes) | The mounts (volumes, bind mounts and so on). | Core spec |

### Deploy (`services.<name>.deploy`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `mode` | The replication model. | Core spec |
| `endpoint_mode` | The service discovery method for external clients. | Core spec |
| `replicas` | The number of containers to run, when `Mode` is `DeployMode.Replicated` (the default). | Core spec |
| `labels` | The labels attached to the service on the platform, not to any of its containers. | Core spec |
| `placement` | Constraints and preferences for the platform's choice of node. | Core spec |
| `resources` | The physical resource constraints and reservations for the containers. | Core spec |
| `restart_policy` | How and whether to restart the containers when they exit. | Core spec |
| `rollback_config` | How to roll the service back after a failed update. | Core spec |
| `update_config` | How to roll out an update to the service. | Core spec |

### Deploy placement (`services.<name>.deploy.placement`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `constraints` | The properties a node must have to run the service. | Core spec |
| `preferences` | The node label values to spread tasks evenly across. | Core spec |
| `max_replicas_per_node` | The maximum number of replicas of the service on a single node. | Core spec |

### Deploy resources (`services.<name>.deploy.resources`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `limits` | The upper bound on what a container may use. | Core spec |
| `reservations` | What the platform must guarantee a container can allocate. | Core spec |

### Deploy resource limits (`services.<name>.deploy.resources.limits`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `cpus` | The number of CPU cores the container can use. | Core spec |
| `memory` | The amount of memory the container can allocate, as a compose-spec byte value such as `50M`. | Core spec |
| `pids` | The container's PID limit. | Core spec |

### Deploy resource reservations (`services.<name>.deploy.resources.reservations`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `cpus` | The number of CPU cores the container can use. | Core spec |
| `memory` | The amount of memory the container can allocate, as a compose-spec byte value such as `20M`. | Core spec |
| `pids` | The container's PID limit. | Core spec |
| `devices` | The devices to reserve. | Core spec |

### Deploy device reservation (`services.<name>.deploy.resources.reservations.devices`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `capabilities` | The capabilities the device must satisfy. | Core spec |
| `driver` | The driver to reserve the device through. | Core spec |
| `count` | The number of devices to reserve. | Core spec |
| `device_ids` | The IDs of the specific devices to reserve. | Core spec |
| `options` | Driver-specific options as key-value pairs. | Core spec |

### Deploy restart policy (`services.<name>.deploy.restart_policy`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `condition` | When a container is restarted. | Core spec |
| `delay` | How long to wait between restart attempts, as a compose-spec duration. | Core spec |
| `max_attempts` | The maximum number of restart attempts before giving up. | Core spec |
| `window` | How long to wait before deciding a restart has succeeded, as a compose-spec duration. | Core spec |

### Deploy rollback configuration (`services.<name>.deploy.rollback_config`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `parallelism` | The number of containers to roll back at a time. | Core spec |
| `delay` | The time to wait between each group's rollback, as a compose-spec duration. | Core spec |
| `failure_action` | What to do if the rollback itself fails. | Core spec |
| `monitor` | How long to monitor each task for failure after it is updated, as a compose-spec duration. | Core spec |
| `max_failure_ratio` | The failure rate to tolerate during the rollback, from `0` to `1`. | Core spec |
| `order` | The order containers are stopped and started in. | Core spec |

### Deploy update configuration (`services.<name>.deploy.update_config`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `parallelism` | The number of containers to update at a time. | Core spec |
| `delay` | The time to wait between updating each group of containers, as a compose-spec duration. | Core spec |
| `failure_action` | What to do if the update fails. | Core spec |
| `monitor` | How long to monitor each task for failure after it is updated, as a compose-spec duration. | Core spec |
| `max_failure_ratio` | The failure rate to tolerate during the update, from `0` to `1`. | Core spec |
| `order` | The order containers are stopped and started in. | Core spec |

### Healthcheck (`services.<name>.healthcheck`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`disable`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | Whether the healthcheck set by the image is disabled. | Core spec |
| [`interval`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | The time between checks, as a compose-spec duration such as `1m30s`. | Core spec |
| [`retries`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | The number of consecutive failures before the container is considered unhealthy. | Core spec |
| [`start_interval`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | The time between checks during the start period, as a compose-spec duration. | 2.20.2+ |
| [`start_period`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | How long the container has to start before failed checks count, as a compose-spec duration. | Core spec |
| [`test`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | The command that checks the container's health. | Core spec |
| [`timeout`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck) | How long a single check may run before it counts as failed, as a compose-spec duration. | Core spec |

### Service dependency, long syntax (`services.<name>.depends_on.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`condition`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax) | When the dependency is considered satisfied. | Core spec |
| [`restart`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax) | Whether this service is restarted when the dependency is restarted by a Compose operation. | 2.17.0+ |
| [`required`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax) | Whether the dependency must be available. When `false`, Compose only warns if it is missing. | 2.20.0+ |

### Env file entry, long syntax (`services.<name>.env_file`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`format`](https://github.com/compose-spec/compose-spec/blob/main/05-services.md#format) | The alternative parsing format for the file. | 2.30.0+ |

### Extends, long syntax (`services.<name>.extends`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `service` | The name of the service being referenced as a base. | Core spec |
| `file` | The Compose file the referenced service is defined in. | Core spec |

### Logging (`services.<name>.logging`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `driver` | The logging driver. | Core spec |
| `options` | Driver-specific options as key-value pairs. | Core spec |

### Port, long syntax (`services.<name>.ports`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `target` | The container port. | Core spec |
| `app_protocol` | The application protocol this port is used for, such as `http`. | 2.26.0+ |
| `host_ip` | The host IP address to bind to. | Core spec |
| `mode` | How the port is published. | Core spec |
| `name` | A human-readable name documenting the port's use. | Core spec |
| `protocol` | The port protocol. | Core spec |
| `published` | The publicly exposed port, or a range such as `8000-9000`. | Core spec |

### Service network attachment (`services.<name>.networks.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `aliases` | Alternative hostnames for the service on this network. | Core spec |
| `driver_opts` | Driver-specific options as key-value pairs. | 2.27.1+ |
| `gw_priority` | The priority for selecting this network as the default gateway. | 2.33.0+ |
| `interface_name` | The network interface name used to connect to this network. | 2.36.0+ |
| `ipv4_address` | A static IPv4 address for the container. | Core spec |
| `ipv6_address` | A static IPv6 address for the container. | Core spec |
| `link_local_ips` | Link-local IP addresses. | Core spec |
| `mac_address` | The MAC address used when connecting to this network. | 2.24.0+ |
| `priority` | The order in which Compose connects the service's containers to its networks. | Core spec |

### Mount, long syntax (`services.<name>.volumes`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `type` | The kind of mount. | Core spec |
| `target` | The path in the container where the mount appears. | Core spec |
| `bind` | Extra options for a bind mount. | Core spec |
| `consistency` | The consistency requirements of the mount. | Core spec |
| `image` | Extra options for an image mount. | 2.35.0+ |
| `read_only` | Whether the mount is read-only. | Core spec |
| `source` | The source of the mount. | Core spec |
| `tmpfs` | Extra options for a tmpfs mount. | Core spec |
| `volume` | Extra options for a named volume mount. | Core spec |

### Bind mount options (`services.<name>.volumes[].bind`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `create_host_path` | Whether to create a directory at the source path on the host if it does not exist. | Core spec |
| `propagation` | The propagation mode used for the bind. | Core spec |
| `selinux` | The SELinux re-labelling option. | Core spec |

### Named volume mount options (`services.<name>.volumes[].volume`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `nocopy` | Whether to disable copying data from the container when the volume is created. | Core spec |
| `subpath` | A path inside the volume to mount instead of the volume root. | Core spec |

### Tmpfs mount options (`services.<name>.volumes[].tmpfs`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `mode` | The file mode as Unix permission bits. | 2.14.0+ |
| `size` | The size of the mount, either a number of bytes or a value with a unit such as `100m`. | Core spec |

### Image mount options (`services.<name>.volumes[].image`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `subpath` | A path inside the image to mount instead of the image root. | 2.35.0+ |

### Networks, top level (`networks.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`attachable`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#attachable) | Whether standalone containers can attach to the network as well as services. | Core spec |
| [`driver`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver) | The network driver to use. | Core spec |
| [`driver_opts`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#driver_opts) | Driver-specific options as key-value pairs. | Core spec |
| [`enable_ipv4`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#enable_ipv4) | Whether IPv4 address assignment is enabled. | 2.33.1+ |
| [`enable_ipv6`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#enable_ipv6) | Whether IPv6 address assignment is enabled. | Core spec |
| [`external`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#external) | Whether the network's lifecycle is managed outside this application. | Core spec |
| [`internal`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#internal) | Whether the network is isolated from the outside world. | Core spec |
| [`labels`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#labels) | The labels attached to the network. | Core spec |
| [`ipam`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#ipam) | A custom IP address management configuration. | Core spec |
| [`name`](https://github.com/compose-spec/compose-spec/blob/main/06-networks.md#name) | The actual Docker network name to use, overriding the default name generated from the project name and the key this network is defined under. | Core spec |

### IPAM (`networks.<name>.ipam`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `config` | The address pools. | Core spec |
| `driver` | A custom IPAM driver. | Core spec |
| `options` | Driver-specific options as key-value pairs. | Core spec |

### IPAM address pool (`networks.<name>.ipam.config`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `aux_addresses` | Auxiliary addresses used by the network driver, as a mapping from hostname to IP address. | Core spec |
| `gateway` | The IPv4 or IPv6 gateway for the subnet. | Core spec |
| `ip_range` | The range of addresses from which container addresses are allocated, in CIDR format. | Core spec |
| `subnet` | The subnet in CIDR format. | Core spec |

### Volumes, top level (`volumes.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`driver`](https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver) | The volume driver to use. | Core spec |
| [`driver_opts`](https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#driver_opts) | Driver-specific options as key-value pairs. | Core spec |
| [`external`](https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#external) | Whether the volume's lifecycle is managed outside this application. | Core spec |
| [`labels`](https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#labels) | The labels attached to the volume. | Core spec |
| [`name`](https://github.com/compose-spec/compose-spec/blob/main/07-volumes.md#name) | The actual Docker volume name to use, overriding the default name generated from the project name and the key this volume is defined under. | Core spec |

### Service secret reference, long syntax (`services.<name>.secrets`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `source` | The name of the secret as defined in the top-level `secrets` section. | Core spec |
| `target` | The name of the file mounted under `/run/secrets/` in the container, or an absolute path for an alternate location. Defaults to the source name if not set. | Core spec |
| `uid` | The numeric user ID that owns the mounted file. | Core spec |
| `gid` | The numeric group ID that owns the mounted file. | Core spec |
| `mode` | The permissions of the mounted file, in octal notation. | Core spec |

### Secrets, top level (`secrets.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`file`](https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md) | The path of the file whose contents become the secret. | Core spec |
| [`environment`](https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md) | The name of the environment variable whose value becomes the secret. | 2.6.0+ |
| [`external`](https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md) | Whether this secret has already been created outside Compose. | Core spec |
| [`name`](https://github.com/compose-spec/compose-spec/blob/main/09-secrets.md) | The actual name of the secret object to look up, overriding the default name generated from the project name and the key this secret is defined under. | Core spec |

### Service config reference, long syntax (`services.<name>.configs`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| `source` | The name of the config as defined in the top-level `configs` section. | Core spec |
| `target` | The path and name of the file mounted in the container. Defaults to `/<source>` if not set. | Core spec |
| `uid` | The numeric user ID that owns the mounted file. | Core spec |
| `gid` | The numeric group ID that owns the mounted file. | Core spec |
| `mode` | The permissions of the mounted file, in octal notation. | Core spec |

### Configs, top level (`configs.<name>`)

| Property | Description | Since Compose |
| :--- | :--- | :--- |
| [`file`](https://github.com/compose-spec/compose-spec/blob/main/08-configs.md) | The path of the file whose contents become the config. | Core spec |
| [`environment`](https://github.com/compose-spec/compose-spec/blob/main/08-configs.md) | The name of the environment variable whose value becomes the config. | Core spec |
| [`content`](https://github.com/compose-spec/compose-spec/blob/main/08-configs.md) | The inline text that becomes the config. | 2.23.1+ |
| [`external`](https://github.com/compose-spec/compose-spec/blob/main/08-configs.md) | Whether this config has already been created outside Compose. | Core spec |
| [`name`](https://github.com/compose-spec/compose-spec/blob/main/08-configs.md) | The actual name of the config object to look up, overriding the default name generated from the project name and the key this config is defined under. | Core spec |

<!-- supported-properties:end -->

Not listed here yet? Check the [milestones](https://github.com/seanzi86/DockerComposeFluent/milestones) for
what's planned, or [open an issue](https://github.com/seanzi86/DockerComposeFluent/issues/new).
