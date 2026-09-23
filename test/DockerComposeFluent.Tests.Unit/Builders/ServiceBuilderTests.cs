using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ServiceBuilderTests
    {
        [Fact]
        public void Build_WithNothingSet_ProducesEmptyDefinition()
        {
            Assert.Equal(new ServiceDefinition(), new ServiceBuilder().Build());
        }

        [Fact]
        public void With_Methods_SetProperties()
        {
            ServiceDefinition service = new ServiceBuilder().WithImage("nginx").WithContainerName("web").Build();

            Assert.Equal("nginx", service.Image);
            Assert.Equal("web", service.ContainerName);
        }

        [Fact]
        public void Build_ReturnsSnapshot_UnaffectedByLaterChanges()
        {
            ServiceBuilder builder = new ServiceBuilder().WithImage("nginx");
            ServiceDefinition first = builder.Build();

            builder.WithImage("apache");

            Assert.Equal("nginx", first.Image);
        }

        [Fact]
        public void BlankValues_Throw()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithImage(" "));
            Assert.Throws<ArgumentException>(() => builder.WithContainerName(""));
        }
    }
}
