using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class NetworkAndVolumeBuilderTests
    {
        [Fact]
        public void NetworkBuilder_SetsProperties()
        {
            NetworkDefinition network = new NetworkBuilder().WithDriver("bridge").WithName("front").Build();

            Assert.Equal("bridge", network.Driver);
            Assert.Equal("front", network.Name);
        }

        [Fact]
        public void VolumeBuilder_SetsProperties()
        {
            VolumeDefinition volume = new VolumeBuilder().WithDriver("local").WithName("data").Build();

            Assert.Equal("local", volume.Driver);
            Assert.Equal("data", volume.Name);
        }

        [Fact]
        public void BlankValues_Throw()
        {
            Assert.Throws<ArgumentException>(() => new NetworkBuilder().WithDriver(""));
            Assert.Throws<ArgumentException>(() => new NetworkBuilder().WithName(" "));
            Assert.Throws<ArgumentException>(() => new VolumeBuilder().WithDriver(""));
            Assert.Throws<ArgumentException>(() => new VolumeBuilder().WithName(" "));
        }
    }
}
