using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class DeviceReservationBuilderTests
    {
        [Fact]
        public void WithCapability_AddsIt()
        {
            DeviceReservationDefinition device = new DeviceReservationBuilder().WithCapabilities("gpu").Build();

            Assert.Equal(new[] { "gpu" }, device.Capabilities);
        }

        [Fact]
        public void WithCapabilities_AddsThem()
        {
            DeviceReservationDefinition device = new DeviceReservationBuilder().WithCapabilities(new[] { "gpu", "tpu" }).Build();

            Assert.Equal(new[] { "gpu", "tpu" }, device.Capabilities);
        }

        [Fact]
        public void WithDriver_SetsIt()
        {
            DeviceReservationDefinition device = new DeviceReservationBuilder().WithCapabilities("gpu").WithDriver("nvidia").Build();

            Assert.Equal("nvidia", device.Driver);
        }

        [Fact]
        public void WithCount_SetsIt()
        {
            DeviceReservationDefinition device = new DeviceReservationBuilder().WithCapabilities("gpu").WithCount(2).Build();

            Assert.Equal(2, device.Count);
            Assert.Empty(device.DeviceIds);
        }

        [Fact]
        public void WithDeviceId_AddsIt()
        {
            DeviceReservationDefinition device = new DeviceReservationBuilder().WithCapabilities("gpu").WithDeviceIds("GPU-1").Build();

            Assert.Equal(new[] { "GPU-1" }, device.DeviceIds);
            Assert.Null(device.Count);
        }

        [Fact]
        public void WithOption_SetsIt()
        {
            DeviceReservationDefinition device = new DeviceReservationBuilder().WithCapabilities("gpu").WithOption("virtualization", "false").Build();

            Assert.Equal("false", device.Options["virtualization"]);
        }

        [Fact]
        public void Build_NoCapability_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new DeviceReservationBuilder().Build());
        }

        [Fact]
        public void Build_CountAndDeviceId_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new DeviceReservationBuilder().WithCapabilities("gpu").WithCount(1).WithDeviceIds("GPU-1").Build());
        }

        [Fact]
        public void WithCount_NotPositive_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DeviceReservationBuilder().WithCount(0));
        }

        [Fact]
        public void InvalidInput_Throws()
        {
            DeviceReservationBuilder builder = new DeviceReservationBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithCapabilities(" "));
            Assert.Throws<ArgumentException>(() => builder.WithDriver(" "));
            Assert.Throws<ArgumentException>(() => builder.WithDeviceIds(" "));
        }
    }
}
