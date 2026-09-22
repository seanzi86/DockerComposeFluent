using System.Collections.Generic;

namespace DockerComposeFluent.Models;

public sealed record DockerComposeFile
{
    public string? Name { get; init; }

    public IReadOnlyDictionary<string, ServiceDefinition> Services { get; init; } = new Dictionary<string, ServiceDefinition>();

    public IReadOnlyDictionary<string, NetworkDefinition> Networks { get; init; } = new Dictionary<string, NetworkDefinition>();

    public IReadOnlyDictionary<string, VolumeDefinition> Volumes { get; init; } = new Dictionary<string, VolumeDefinition>();
}
