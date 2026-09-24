using System;
using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class NetworkAttachmentBuilderTests
    {
        [Fact]
        public void Build_SetsEverySetting()
        {
            NetworkAttachment attachment = new NetworkAttachmentBuilder()
                .WithAlias("db")
                .WithDriverOption("foo", "bar")
                .WithGatewayPriority(1)
                .WithInterfaceName("eth0")
                .WithIpv4Address("172.16.238.10")
                .WithIpv6Address("2001:3984:3989::10")
                .WithLinkLocalIp("57.123.22.11")
                .WithMacAddress("02:42:ac:11:00:02")
                .WithPriority(100)
                .Build();

            Assert.Equal(new[] { "db" }, attachment.Aliases);
            Assert.Equal("bar", attachment.DriverOptions["foo"]);
            Assert.Equal(1, attachment.GatewayPriority);
            Assert.Equal("eth0", attachment.InterfaceName);
            Assert.Equal("172.16.238.10", attachment.Ipv4Address);
            Assert.Equal("2001:3984:3989::10", attachment.Ipv6Address);
            Assert.Equal(new[] { "57.123.22.11" }, attachment.LinkLocalIps);
            Assert.Equal("02:42:ac:11:00:02", attachment.MacAddress);
            Assert.Equal(100, attachment.Priority);
            Assert.False(attachment.IsEmpty);
        }

        [Fact]
        public void Build_WithNothingSet_IsEmpty()
        {
            Assert.True(new NetworkAttachmentBuilder().Build().IsEmpty);
        }

        [Fact]
        public void Aliases_AndLinkLocalIps_Append()
        {
            NetworkAttachment attachment = new NetworkAttachmentBuilder()
                .WithAlias("a")
                .WithAliases(new[] { "b", "c" })
                .WithLinkLocalIp("169.254.1.1")
                .WithLinkLocalIps(new[] { "fe80::1" })
                .Build();

            Assert.Equal(new[] { "a", "b", "c" }, attachment.Aliases);
            Assert.Equal(new[] { "169.254.1.1", "fe80::1" }, attachment.LinkLocalIps);
        }

        [Fact]
        public void DriverOptions_SameKeyReplaces()
        {
            NetworkAttachment attachment = new NetworkAttachmentBuilder()
                .WithDriverOption("foo", "1")
                .WithDriverOptions(new[] { new KeyValuePair<string, string>("foo", "2"), new KeyValuePair<string, string>("baz", "3") })
                .Build();

            Assert.Equal(2, attachment.DriverOptions.Count);
            Assert.Equal("2", attachment.DriverOptions["foo"]);
        }

        [Fact]
        public void Build_ReturnsSnapshot_UnaffectedByLaterChanges()
        {
            NetworkAttachmentBuilder builder = new NetworkAttachmentBuilder().WithAlias("a");
            NetworkAttachment first = builder.Build();

            builder.WithAlias("b");

            Assert.Single(first.Aliases);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1.2.3")]
        [InlineData("999.1.1.1")]
        [InlineData("::1")]
        [InlineData("host")]
        [InlineData("")]
        [InlineData(" ")]
        public void WithIpv4Address_Invalid_Throws(string address)
        {
            Assert.Throws<ArgumentException>(() => new NetworkAttachmentBuilder().WithIpv4Address(address));
        }

        [Theory]
        [InlineData("172.16.0.1")]
        [InlineData("gibberish")]
        [InlineData("")]
        public void WithIpv6Address_Invalid_Throws(string address)
        {
            Assert.Throws<ArgumentException>(() => new NetworkAttachmentBuilder().WithIpv6Address(address));
        }

        [Fact]
        public void OtherInvalidArguments_Throw()
        {
            NetworkAttachmentBuilder builder = new NetworkAttachmentBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithAlias(" "));
            Assert.Throws<ArgumentException>(() => builder.WithInterfaceName(""));
            Assert.Throws<ArgumentException>(() => builder.WithMacAddress(" "));
            Assert.Throws<ArgumentException>(() => builder.WithLinkLocalIp("nope"));
            Assert.Throws<ArgumentException>(() => builder.WithDriverOption(" ", "x"));
            Assert.Throws<ArgumentNullException>(() => builder.WithDriverOption("a", null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithAliases(null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithLinkLocalIps(null!));
            Assert.Throws<ArgumentNullException>(() => builder.WithDriverOptions(null!));
        }
    }
}
