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

        [Fact]
        public void NetworkBuilder_SetsTheRemainingTopLevelFields()
        {
            NetworkDefinition network = new NetworkBuilder()
                .WithAttachable(true)
                .WithDriverOption("mtu", "1400")
                .WithEnableIpv4(false)
                .WithEnableIpv6(true)
                .WithExternal(false)
                .WithInternal(true)
                .WithIpam(ipam => ipam.WithConfig("10.0.0.0/8"))
                .Build();

            Assert.Equal(true, network.Attachable);
            Assert.Equal("1400", network.DriverOptions["mtu"]);
            Assert.Equal(false, network.EnableIpv4);
            Assert.Equal(true, network.EnableIpv6);
            Assert.Equal(false, network.External);
            Assert.Equal(true, network.Internal);
            Assert.Equal("10.0.0.0/8", network.Ipam!.Config[0].Subnet);
        }

        [Fact]
        public void NetworkBuilder_RawIpamOverload_IsUsedAsGiven()
        {
            IpamDefinition ipam = new IpamDefinition { Driver = "default" };

            Assert.Same(ipam, new NetworkBuilder().WithIpam(ipam).Build().Ipam);
        }

        [Fact]
        public void NetworkBuilder_DriverOptions_SameKeyReplaces()
        {
            NetworkDefinition network = new NetworkBuilder().WithDriverOption("a", "1").WithDriverOption("a", "2").Build();

            Assert.Equal("2", network.DriverOptions["a"]);
            Assert.Single(network.DriverOptions);
        }

        [Fact]
        public void NetworkBuilder_InvalidArguments_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new NetworkBuilder().WithIpam((IpamDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => new NetworkBuilder().WithIpam((Action<IpamBuilder>)null!));
            Assert.Throws<ArgumentException>(() => new NetworkBuilder().WithDriverOption(" ", "x"));
        }
    }
}
