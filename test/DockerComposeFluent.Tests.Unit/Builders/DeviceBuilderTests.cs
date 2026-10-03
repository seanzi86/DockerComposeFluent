using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class DeviceBuilderTests
    {
        [Fact]
        public void WithCapability_AddsIt()
        {
            DeviceDefinition device = new DeviceBuilder().WithCapability("gpu").Build();

            Assert.Equal(new[] { "gpu" }, device.Capabilities);
        }

        [Fact]
        public void WithCapabilities_AddsThem()
        {
            DeviceDefinition device = new DeviceBuilder().WithCapabilities(new[] { "gpu", "tpu" }).Build();

            Assert.Equal(new[] { "gpu", "tpu" }, device.Capabilities);
        }

        [Fact]
        public void WithDriver_SetsIt()
        {
            DeviceDefinition device = new DeviceBuilder().WithCapability("gpu").WithDriver("nvidia").Build();

            Assert.Equal("nvidia", device.Driver);
        }

        [Fact]
        public void WithCount_SetsIt()
        {
            DeviceDefinition device = new DeviceBuilder().WithCapability("gpu").WithCount(2).Build();

            Assert.Equal(2, device.Count);
            Assert.Empty(device.DeviceIds);
        }

        [Fact]
        public void WithDeviceId_AddsIt()
        {
            DeviceDefinition device = new DeviceBuilder().WithCapability("gpu").WithDeviceId("GPU-1").Build();

            Assert.Equal(new[] { "GPU-1" }, device.DeviceIds);
            Assert.Null(device.Count);
        }

        [Fact]
        public void WithOption_SetsIt()
        {
            DeviceDefinition device = new DeviceBuilder().WithCapability("gpu").WithOption("virtualization", "false").Build();

            Assert.Equal("false", device.Options["virtualization"]);
        }

        [Fact]
        public void Build_NoCapability_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new DeviceBuilder().Build());
        }

        [Fact]
        public void Build_CountAndDeviceId_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new DeviceBuilder().WithCapability("gpu").WithCount(1).WithDeviceId("GPU-1").Build());
        }

        [Fact]
        public void WithCount_NotPositive_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DeviceBuilder().WithCount(0));
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            DeviceBuilder builder = new DeviceBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithCapability(" "));
            Assert.Throws<ArgumentException>(() => builder.WithDriver(" "));
            Assert.Throws<ArgumentException>(() => builder.WithDeviceId(" "));
        }
    }
}
