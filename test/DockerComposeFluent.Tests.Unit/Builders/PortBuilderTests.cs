using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class PortBuilderTests
    {
        [Fact]
        public void Build_SetsEveryField()
        {
            PortDefinition port = new PortBuilder()
                .WithTarget(80)
                .WithPublished(8080)
                .WithHostIp("127.0.0.1")
                .WithProtocol(PortProtocol.Udp)
                .WithAppProtocol("http")
                .WithMode(PortMode.Host)
                .WithName("web")
                .Build();

            Assert.Equal(80, port.Target);
            Assert.Equal("8080", port.Published);
            Assert.Equal("127.0.0.1", port.HostIp);
            Assert.Equal(PortProtocol.Udp, port.Protocol);
            Assert.Equal("http", port.AppProtocol);
            Assert.Equal(PortMode.Host, port.Mode);
            Assert.Equal("web", port.Name);
        }

        [Fact]
        public void Build_WithOnlyTarget_LeavesOtherFieldsUnset()
        {
            PortDefinition port = new PortBuilder().WithTarget(80).Build();

            Assert.Null(port.Published);
            Assert.Null(port.HostIp);
            Assert.Null(port.Protocol);
            Assert.Null(port.AppProtocol);
            Assert.Null(port.Mode);
            Assert.Null(port.Name);
        }

        [Fact]
        public void Build_WithoutTarget_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new PortBuilder().WithPublished(8080).Build());
        }

        [Fact]
        public void WithPublished_Range_WritesStartAndEnd()
        {
            PortDefinition port = new PortBuilder().WithTarget(80).WithPublished(8000, 9000).Build();

            Assert.Equal("8000-9000", port.Published);
        }

        [Fact]
        public void WithPublished_RangeEndingBeforeStart_Throws()
        {
            Assert.Throws<ArgumentException>(() => new PortBuilder().WithPublished(9000, 8000));
        }

        [Fact]
        public void InvalidPorts_Throw()
        {
            PortBuilder builder = new PortBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithTarget(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPublished(65536));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPublished(0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPublished(10, 70000));
        }

        [Fact]
        public void BlankStrings_Throw()
        {
            PortBuilder builder = new PortBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithHostIp(" "));
            Assert.Throws<ArgumentException>(() => builder.WithAppProtocol(""));
            Assert.Throws<ArgumentException>(() => builder.WithName(" "));
        }
    }
}
