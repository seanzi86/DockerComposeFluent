using System;
using System.Collections.Generic;
using System.IO;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class DockerComposeBuilderTests
    {
        [Fact]
        public void Build_WithNothingSet_ProducesEmptyFile()
        {
            DockerComposeFile file = new DockerComposeBuilder().Build();

            Assert.Null(file.Name);
            Assert.Empty(file.Services);
            Assert.Empty(file.Networks);
            Assert.Empty(file.Volumes);
        }

        [Fact]
        public void WithName_SetsName()
        {
            DockerComposeFile file = new DockerComposeBuilder().WithName("demo").Build();

            Assert.Equal("demo", file.Name);
        }

        [Fact]
        public void WithService_ModelOverload_AddsService()
        {
            ServiceDefinition service = new ServiceDefinition { Image = "nginx" };

            DockerComposeFile file = new DockerComposeBuilder().WithService("web", service).Build();

            Assert.Same(service, file.Services["web"]);
        }

        [Fact]
        public void WithService_ConfigureOverload_AddsService()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("web", service => service.WithImage("nginx").WithContainerName("web-container"))
                .Build();

            Assert.Equal("nginx", file.Services["web"].Image);
            Assert.Equal("web-container", file.Services["web"].ContainerName);
        }

        [Fact]
        public void WithNetwork_BothOverloads_AddNetworks()
        {
            NetworkDefinition existing = new NetworkDefinition { Driver = "overlay" };

            DockerComposeFile file = new DockerComposeBuilder()
                .WithNetwork("back", existing)
                .WithNetwork("front", network => network.WithDriver("bridge").WithName("front_net"))
                .Build();

            Assert.Same(existing, file.Networks["back"]);
            Assert.Equal("bridge", file.Networks["front"].Driver);
            Assert.Equal("front_net", file.Networks["front"].Name);
        }

        [Fact]
        public void WithVolume_BothOverloads_AddVolumes()
        {
            VolumeDefinition existing = new VolumeDefinition { Driver = "local" };

            DockerComposeFile file = new DockerComposeBuilder()
                .WithVolume("cache", existing)
                .WithVolume("data", volume => volume.WithDriver("local").WithName("demo_data"))
                .Build();

            Assert.Same(existing, file.Volumes["cache"]);
            Assert.Equal("local", file.Volumes["data"].Driver);
            Assert.Equal("demo_data", file.Volumes["data"].Name);
        }

        [Fact]
        public void WithService_SameNameTwice_LastOneWins()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithService("web", service => service.WithImage("nginx:1"))
                .WithService("web", service => service.WithImage("nginx:2"))
                .Build();

            Assert.Single(file.Services);
            Assert.Equal("nginx:2", file.Services["web"].Image);
        }

        [Fact]
        public void WithService_InLoop_AddsEachService()
        {
            DockerComposeBuilder builder = new DockerComposeBuilder();

            foreach (string name in new[] { "a", "b", "c" })
            {
                builder.WithService(name, service => service.WithImage(name));
            }

            Assert.Equal(new[] { "a", "b", "c" }, new List<string>(builder.Build().Services.Keys));
        }

        [Fact]
        public void Build_ReturnsSnapshot_UnaffectedByLaterChanges()
        {
            DockerComposeBuilder builder = new DockerComposeBuilder().WithService("web", service => service.WithImage("nginx"));
            DockerComposeFile first = builder.Build();

            builder.WithService("db", service => service.WithImage("postgres"));

            Assert.Single(first.Services);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void BlankNames_Throw(string? name)
        {
            DockerComposeBuilder builder = new DockerComposeBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithName(name!));
            Assert.Throws<ArgumentException>(() => builder.WithService(name!, new ServiceDefinition()));
            Assert.Throws<ArgumentException>(() => builder.WithNetwork(name!, new NetworkDefinition()));
            Assert.Throws<ArgumentException>(() => builder.WithVolume(name!, new VolumeDefinition()));
        }

        [Fact]
        public void NullArguments_Throw()
        {
            DockerComposeBuilder builder = new DockerComposeBuilder();

            Assert.Throws<ArgumentNullException>(() => builder.WithService("web", (ServiceDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithService("web", (Action<ServiceBuilder>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithNetwork("net", (NetworkDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithNetwork("net", (Action<NetworkBuilder>)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolume("vol", (VolumeDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithVolume("vol", (Action<VolumeBuilder>)null!));
        }

        [Fact]
        public void Build_ThenToYaml_MatchesGoldenFixture()
        {
            DockerComposeFile file = new DockerComposeBuilder()
                .WithName("demo")
                .WithService("web", service => service.WithContainerName("web-container").WithImage("nginx:latest"))
                .WithNetwork("frontend", network => network.WithDriver("bridge"))
                .WithVolume("data", volume => volume.WithDriver("local").WithName("demo_data"))
                .Build();

            string expected = File.ReadAllText(Path.Combine("Serialisation", "Fixtures", "basic.yml")).Replace("\r\n", "\n");

            Assert.Equal(expected, file.ToYaml().Replace("\r\n", "\n"));
        }
    }
}
