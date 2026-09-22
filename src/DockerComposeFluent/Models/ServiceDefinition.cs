namespace DockerComposeFluent.Models;

public sealed record ServiceDefinition
{
    public string? Image { get; init; }

    public string? ContainerName { get; init; }
}
