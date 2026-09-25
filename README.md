# DockerComposeFluent

A fluent .NET API for generating `docker-compose.yml` files.

[![CI](https://github.com/seanzi86/DockerComposeFluent/actions/workflows/ci.yml/badge.svg)](https://github.com/seanzi86/DockerComposeFluent/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/DockerComposeFluent.svg)](https://www.nuget.org/packages/DockerComposeFluent)
[![License: MIT](https://img.shields.io/github/license/seanzi86/DockerComposeFluent)](https://github.com/seanzi86/DockerComposeFluent/blob/main/LICENSE)

> **Status:** pre-1.0. The API may still change between minor versions.

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

- **Services:** `image`, `container_name`, `command`, `entrypoint`, `environment`, `networks`, `ports`, `restart` and `volumes`.
- **Top level:** `name`, `networks` (including `ipam`) and `volumes`.

Each property accepts the forms Compose itself accepts, chosen by the overload you call. For example `WithPort("8080:80")` writes the short syntax, `WithPort(8080, 80)` writes the long syntax, and `WithPort(port => port.WithTarget(80).WithHostIp("127.0.0.1"))` gives full control.

More of the compose-spec is planned; see the [milestones](https://github.com/seanzi86/DockerComposeFluent/milestones).

## Docker Compose versions

The library targets the current Docker Compose, and the generated files are validated with `docker compose config` in CI. A few options need a newer Compose than others, which is noted in the API documentation.

## Contributing

See [CONTRIBUTING.md](https://github.com/seanzi86/DockerComposeFluent/blob/main/CONTRIBUTING.md).

## Licence

[MIT](https://github.com/seanzi86/DockerComposeFluent/blob/main/LICENSE)
