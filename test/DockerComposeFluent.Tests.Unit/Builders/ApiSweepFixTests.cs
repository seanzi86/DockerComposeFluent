using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ApiSweepFixTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ServiceSecretAndConfigShortcuts_RejectABlankSource(string? source)
        {
            Assert.ThrowsAny<ArgumentException>(() => new ServiceBuilder().WithSecret(source!));
            Assert.ThrowsAny<ArgumentException>(() => new ServiceBuilder().WithSecret(source!, "target"));
            Assert.ThrowsAny<ArgumentException>(() => new ServiceBuilder().WithConfig(source!));
            Assert.ThrowsAny<ArgumentException>(() => new ServiceBuilder().WithConfig(source!, "target"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ServiceSecretAndConfigShortcuts_RejectABlankTarget(string? target)
        {
            Assert.ThrowsAny<ArgumentException>(() => new ServiceBuilder().WithSecret("s", target!));
            Assert.ThrowsAny<ArgumentException>(() => new ServiceBuilder().WithConfig("c", target!));
        }

        [Fact]
        public void HandBuiltReferencesWithoutASource_AreRejectedWhenAdded()
        {
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithSecret(new SecretReference()));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithConfig(new ConfigReference()));
            Assert.Throws<ArgumentException>(() => new BuildBuilder().WithSecret(new SecretReference()));
        }

        [Fact]
        public void NameIsValidatedBeforeConfigureRuns()
        {
            // The configure action is incomplete, so building it would throw InvalidOperationException.
            // The bad name must be reported first.
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithService(" ", s => { }));
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithNetwork(" ", n => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithVolume(" ", v => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithSecret(" ", s => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new DockerComposeBuilder().WithConfig(" ", c => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithDependsOn(" ", d => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithNetwork(" ", n => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithUlimit("Bad Name", u => throw new InvalidOperationException()));
            Assert.Throws<ArgumentException>(() => new BuildBuilder().WithUlimit("Bad Name", u => throw new InvalidOperationException()));
        }

        [Fact]
        public void BuildUlimit_ActionOverload_MatchesTheDefinitionOverload()
        {
            BuildDefinition viaAction = new BuildBuilder().WithUlimit("nofile", u => u.WithSoft(1).WithHard(2)).Build();
            BuildDefinition viaRaw = new BuildBuilder().WithUlimit("nofile", 1, 2).Build();

            Assert.Equal(viaRaw.Ulimits["nofile"], viaAction.Ulimits["nofile"]);
        }

        [Fact]
        public void BlkioLimits_ActionOverloads_MatchTheDefinitionOverloads()
        {
            BlkioConfigDefinition viaAction = new BlkioConfigBuilder()
                .WithDeviceReadBps(l => l.WithPath("/dev/a").WithRate("1mb"))
                .WithDeviceReadIops(l => l.WithPath("/dev/a").WithRate("10"))
                .WithDeviceWriteBps(l => l.WithPath("/dev/a").WithRate("2mb"))
                .WithDeviceWriteIops(l => l.WithPath("/dev/a").WithRate("20"))
                .Build();
            BlkioConfigDefinition viaRaw = new BlkioConfigBuilder()
                .WithDeviceReadBps("/dev/a", "1mb")
                .WithDeviceReadIops("/dev/a", "10")
                .WithDeviceWriteBps("/dev/a", "2mb")
                .WithDeviceWriteIops("/dev/a", "20")
                .Build();

            Assert.Equal(viaRaw.DeviceReadBps[0], viaAction.DeviceReadBps[0]);
            Assert.Equal(viaRaw.DeviceReadIops[0], viaAction.DeviceReadIops[0]);
            Assert.Equal(viaRaw.DeviceWriteBps[0], viaAction.DeviceWriteBps[0]);
            Assert.Equal(viaRaw.DeviceWriteIops[0], viaAction.DeviceWriteIops[0]);
            Assert.Throws<ArgumentNullException>(() => new BlkioConfigBuilder().WithDeviceReadBps((Action<BlkioLimitBuilder>)null!));
        }
    }
}
