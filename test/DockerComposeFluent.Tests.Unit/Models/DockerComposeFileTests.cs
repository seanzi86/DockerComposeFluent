using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Models;

public class DockerComposeFileTests
{
    [Fact]
    public void DefaultCollections_AreEmpty_NotNull()
    {
        DockerComposeFile file = new();

        Assert.Empty(file.Services);
        Assert.Empty(file.Networks);
        Assert.Empty(file.Volumes);
    }

    [Fact]
    public void With_ProducesNewInstance_LeavingOriginalUnchanged()
    {
        DockerComposeFile original = new() { Name = "original" };

        DockerComposeFile updated = original with
        {
            Name = "updated",
            Services = new Dictionary<string, ServiceDefinition>
            {
                ["web"] = new ServiceDefinition { Image = "nginx" }
            }
        };

        Assert.Equal("original", original.Name);
        Assert.Empty(original.Services);

        Assert.Equal("updated", updated.Name);
        Assert.Equal("nginx", updated.Services["web"].Image);
    }
}
