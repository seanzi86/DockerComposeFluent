using System;
using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class IpamBuilderTests
    {
        [Fact]
        public void IpamBuilder_SetsDriverConfigAndOptions()
        {
            IpamDefinition ipam = new IpamBuilder()
                .WithDriver("default")
                .WithConfig("172.28.0.0/16")
                .WithConfig(config => config.WithSubnet("2001:db8::/32").WithGateway("2001:db8::1"))
                .WithOption("foo", "bar")
                .Build();

            Assert.Equal("default", ipam.Driver);
            Assert.Equal(2, ipam.Config.Count);
            Assert.Equal("172.28.0.0/16", ipam.Config[0].Subnet);
            Assert.Equal("2001:db8::1", ipam.Config[1].Gateway);
            Assert.Equal("bar", ipam.Options["foo"]);
        }

        [Fact]
        public void IpamBuilder_RawConfigOverload_IsUsedAsGiven()
        {
            IpamConfigDefinition config = new IpamConfigDefinition { Subnet = "10.0.0.0/8" };

            IpamDefinition ipam = new IpamBuilder().WithConfig(config).Build();

            Assert.Same(config, ipam.Config[0]);
        }

        [Fact]
        public void IpamConfigBuilder_SetsEveryField()
        {
            IpamConfigDefinition config = new IpamConfigBuilder()
                .WithSubnet("172.28.0.0/16")
                .WithIpRange("172.28.5.0/24")
                .WithGateway("172.28.5.254")
                .WithAuxAddress("host1", "172.28.1.5")
                .WithAuxAddresses(new[] { new KeyValuePair<string, string>("host2", "172.28.1.6") })
                .Build();

            Assert.Equal("172.28.0.0/16", config.Subnet);
            Assert.Equal("172.28.5.0/24", config.IpRange);
            Assert.Equal("172.28.5.254", config.Gateway);
            Assert.Equal(2, config.AuxAddresses.Count);
            Assert.Equal("172.28.1.6", config.AuxAddresses["host2"]);
        }

        [Theory]
        [InlineData("172.28.0.0")]
        [InlineData("172.28.0.0/33")]
        [InlineData("/16")]
        [InlineData("abc/16")]
        [InlineData("2001:db8::/129")]
        [InlineData("172.28.0.0/-1")]
        [InlineData("172.28.0.0/")]
        [InlineData("")]
        public void InvalidCidr_Throws(string cidr)
        {
            Assert.Throws<ArgumentException>(() => new IpamConfigBuilder().WithSubnet(cidr));
            Assert.Throws<ArgumentException>(() => new IpamConfigBuilder().WithIpRange(cidr));
            Assert.Throws<ArgumentException>(() => new IpamBuilder().WithConfig(cidr));
        }

        [Theory]
        [InlineData("172.28.0.0/16")]
        [InlineData("172.28.0.0/0")]
        [InlineData("2001:db8::/32")]
        [InlineData("2001:db8::/128")]
        public void ValidCidr_IsAccepted(string cidr)
        {
            Assert.Equal(cidr, new IpamConfigBuilder().WithSubnet(cidr).Build().Subnet);
        }

        [Fact]
        public void OtherInvalidArguments_Throw()
        {
            Assert.Throws<ArgumentException>(() => new IpamConfigBuilder().WithGateway("nope"));
            Assert.Throws<ArgumentException>(() => new IpamConfigBuilder().WithAuxAddress(" ", "10.0.0.1"));
            Assert.Throws<ArgumentException>(() => new IpamConfigBuilder().WithAuxAddress("h", "nope"));
            Assert.Throws<ArgumentException>(() => new IpamBuilder().WithDriver(" "));
            Assert.Throws<ArgumentNullException>(() => new IpamBuilder().WithConfig((IpamConfigDefinition)null!));
            Assert.Throws<ArgumentNullException>(() => new IpamBuilder().WithConfig((Action<IpamConfigBuilder>)null!));
            Assert.Throws<ArgumentNullException>(() => new IpamBuilder().WithOption("a", null!));
        }
    }
}
