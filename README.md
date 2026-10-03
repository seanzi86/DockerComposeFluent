# DockerComposeFluent

A fluent .NET API for generating `docker-compose.yml` files.

[![CI](https://github.com/seanzi86/DockerComposeFluent/actions/workflows/ci.yml/badge.svg)](https://github.com/seanzi86/DockerComposeFluent/actions/workflows/ci.yml)
[![Coverage Status](https://coveralls.io/repos/github/seanzi86/DockerComposeFluent/badge.svg?branch=main)](https://coveralls.io/github/seanzi86/DockerComposeFluent?branch=main)
[![NuGet](https://img.shields.io/nuget/v/DockerComposeFluent.svg)](https://www.nuget.org/packages/DockerComposeFluent)
[![License: MIT](https://img.shields.io/github/license/seanzi86/DockerComposeFluent)](https://github.com/seanzi86/DockerComposeFluent/blob/main/LICENSE)

> **Status:** pre-1.0. The API may still change between minor versions.

Full documentation, including the API reference and the supported-properties matrix, is at
**[seanzi86.github.io/DockerComposeFluent](https://seanzi86.github.io/DockerComposeFluent/)**.

## Install

```bash
dotnet add package DockerComposeFluent
```

## Quick start

```csharp
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

DockerComposeFile file = new DockerComposeBuilder()
    .WithName("shop")
    .WithService("web", service => service
        .WithImage("nginx:1.27")
        .WithPort(8080, 80)
        .WithVolume("./site", "/usr/share/nginx/html", readOnly: true)
        .WithNetwork("front"))
    .WithService("db", service => service
        .WithImage("postgres:17")
        .WithEnvironment("POSTGRES_DB", "shop")
        .WithEnvironment("POSTGRES_PASSWORD")
        .WithVolume("db-data", "/var/lib/postgresql/data")
        .WithNetwork("back"))
    .WithNetwork("front", network => network.WithDriver("bridge"))
    .WithNetwork("back", network => network.WithInternal(true))
    .WithVolume("db-data", volume => volume.WithDriver("local"))
    .Build();

string yaml = file.ToYaml();
```

`yaml` is:

```yaml
name: "shop"
services:
  "web":
    image: "nginx:1.27"
    networks:
      - "front"
    ports:
      - published: "8080"
        target: 80
    volumes:
      - "./site:/usr/share/nginx/html:ro"
  "db":
    environment:
      "POSTGRES_DB": "shop"
      "POSTGRES_PASSWORD": null
    image: "postgres:17"
    networks:
      - "back"
    volumes:
      - "db-data:/var/lib/postgresql/data"
networks:
  "front":
    driver: "bridge"
  "back":
    internal: true
volumes:
  "db-data":
    driver: "local"
```

## What is supported

- **Services:** `image`, `container_name`, `command`, `depends_on`, `deploy`, `entrypoint`, `env_file`, `environment`, `extends`, `healthcheck`, `label_file`, `labels`, `logging`, `networks`, `platform`, `ports`, `profiles`, `restart`, `secrets`, `configs` and `volumes`, plus the common settings `hostname`, `domainname`, `user`, `working_dir`, `mac_address`, `stop_signal`, `stop_grace_period`, `init`, `privileged`, `read_only`, `stdin_open`, `tty`, `runtime`, `pull_policy`, `pull_refresh_after`, `isolation`, `attach`, `scale`, plus namespace and mode settings (`network_mode`, `ipc`, `pid`, `uts`, `userns_mode`, `cgroup`, `cgroup_parent`), capabilities, security and devices (`cap_add`, `cap_drop`, `security_opt`, `group_add`, `device_cgroup_rules`, `credential_spec`, `devices`, `gpus`), networking (`dns`, `dns_opt`, `dns_search`, `extra_hosts`, `expose`, `links`, `external_links`, `volumes_from`, `tmpfs`) and limits (`cpus` and the other `cpu_*` settings, `cpuset`, `mem_limit`, `mem_reservation`, `memswap_limit`, `mem_swappiness`, `oom_kill_disable`, `oom_score_adj`, `pids_limit`, `shm_size`, `blkio_config`, `storage_opt`, `sysctls`, `ulimits`).
- **Top level:** `name`, `include`, `networks` (including `ipam` and `labels`), `volumes` (including `driver_opts`, `external` and `labels`), `secrets` and `configs`.
- **Extension fields:** every modelled object - the file itself, every service, and every nested setting such as `deploy`, `healthcheck` or a mount's `bind` options - has a `WithExtension(key, value)` for custom `x-` fields, which Compose ignores but many tools and YAML anchors rely on.

Each property accepts the forms Compose itself accepts, chosen by the overload you call. For example `WithPort("8080:80")` writes the short syntax, `WithPort(8080, 80)` writes the long syntax, and `WithPort(port => port.WithTarget(80).WithHostIp("127.0.0.1"))` gives full control.

More of the compose-spec is planned; see the [milestones](https://github.com/seanzi86/DockerComposeFluent/milestones).

## Docker Compose versions

The library targets the current Docker Compose, and the generated files are validated with `docker compose config` in CI. A few options need a newer Compose than others, which is noted in the API documentation.

## Contributing

See [CONTRIBUTING.md](https://github.com/seanzi86/DockerComposeFluent/blob/main/CONTRIBUTING.md).

## Licence

[MIT](https://github.com/seanzi86/DockerComposeFluent/blob/main/LICENSE)
