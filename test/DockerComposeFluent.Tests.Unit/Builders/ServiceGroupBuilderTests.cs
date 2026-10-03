using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Builders
{
    public class ServiceGroupBuilderTests
    {
        [Fact]
        public void StringLists_AppendAndAcceptSeveral()
        {
            ServiceDefinition service = new ServiceBuilder()
                .WithCapAdd("NET_ADMIN").WithCapAdd(new[] { "SYS_TIME" })
                .WithDns("8.8.8.8").WithDnsSearch("example.com")
                .WithLinks("db").WithVolumesFrom("db:ro").WithTmpfs("/run")
                .Build();

            Assert.Equal(new[] { "NET_ADMIN", "SYS_TIME" }, service.CapAdd);
            Assert.Equal(new[] { "8.8.8.8" }, service.Dns);
            Assert.Equal(new[] { "example.com" }, service.DnsSearch);
            Assert.Equal(new[] { "db" }, service.Links);
            Assert.Equal(new[] { "db:ro" }, service.VolumesFrom);
            Assert.Equal(new[] { "/run" }, service.Tmpfs);
        }

        [Fact]
        public void Expose_AcceptsPortNumbersAndRanges()
        {
            ServiceDefinition service = new ServiceBuilder().WithExpose(3000).WithExpose("8000-8010").Build();

            Assert.Equal(new[] { "3000", "8000-8010" }, service.Expose);
            Assert.Throws<ArgumentOutOfRangeException>(() => new ServiceBuilder().WithExpose(0));
        }

        [Fact]
        public void Cgroup_SetsAndRejectsUnknownValues()
        {
            Assert.Equal(CgroupMode.Private, new ServiceBuilder().WithCgroup(CgroupMode.Private).Build().Cgroup);
            Assert.Throws<ArgumentOutOfRangeException>(() => new ServiceBuilder().WithCgroup((CgroupMode)99));
        }

        [Fact]
        public void Cpus_AcceptsTextAndNumbers()
        {
            Assert.Equal("0.5", new ServiceBuilder().WithCpus(0.5).Build().Cpus);
            Assert.Equal("1.25", new ServiceBuilder().WithCpus("1.25").Build().Cpus);
            Assert.Throws<ArgumentOutOfRangeException>(() => new ServiceBuilder().WithCpus(0));
        }

        [Fact]
        public void NumericRanges_AreEnforced()
        {
            ServiceBuilder builder = new ServiceBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithCpuPercent(101));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithCpuCount(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithMemSwappiness(101));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithOomScoreAdj(1001));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPidsLimit(-2));
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithCpuShares(-1));
            Assert.Equal(-1000, builder.WithOomScoreAdj(-1000).Build().OomScoreAdj);
        }

        [Fact]
        public void Sysctl_AcceptsTextAndNumbers_AndSameKeyReplaces()
        {
            ServiceDefinition service = new ServiceBuilder().WithSysctl("a", 1).WithSysctl("a", "2").WithSysctl("b", "x").Build();

            Assert.Equal("2", service.Sysctls["a"]);
            Assert.Equal(2, service.Sysctls.Count);
        }

        [Fact]
        public void ExtraHost_SameHostReplaces()
        {
            ServiceDefinition service = new ServiceBuilder().WithExtraHost("h", "1.1.1.1").WithExtraHost("h", "2.2.2.2").Build();

            Assert.Equal("2.2.2.2", service.ExtraHosts["h"]);
            Assert.Single(service.ExtraHosts);
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithExtraHost(" ", "1.1.1.1"));
        }

        [Fact]
        public void CredentialSpec_NeedsExactlyOneSource()
        {
            Assert.Equal("c", new CredentialSpecBuilder().WithConfig("c").Build().Config);
            Assert.Throws<InvalidOperationException>(() => new CredentialSpecBuilder().Build());
            Assert.Throws<InvalidOperationException>(() => new CredentialSpecBuilder().WithConfig("c").WithFile("f").Build());
        }

        [Fact]
        public void DeviceMapping_DefaultsTargetWhenLongFormIsNeeded()
        {
            DeviceMappingDefinition plain = new DeviceMappingBuilder().WithSource("/dev/a").Build();
            DeviceMappingDefinition withPermissions = new DeviceMappingBuilder().WithSource("/dev/a").WithPermissions("rw").Build();
            DeviceMappingDefinition withExtension = new DeviceMappingBuilder().WithSource("/dev/a").WithExtension("x-n", 1).Build();

            Assert.True(plain.CanBeShort);
            Assert.Equal("/dev/a", withPermissions.Target);
            Assert.True(withPermissions.CanBeShort);
            Assert.False(withExtension.CanBeShort);
            Assert.Equal("/dev/a", withExtension.Target);
        }

        [Fact]
        public void DeviceMapping_Validates()
        {
            Assert.Throws<InvalidOperationException>(() => new DeviceMappingBuilder().Build());
            Assert.Throws<ArgumentException>(() => new DeviceMappingBuilder().WithPermissions("rwx"));
            Assert.Throws<ArgumentException>(() => new DeviceMappingBuilder().WithSource(" "));
        }

        [Fact]
        public void Gpus_AllReplacesAndSpecificRequestsAccumulate()
        {
            ServiceDefinition some = new ServiceBuilder()
                .WithAllGpus()
                .WithGpu(gpu => gpu.WithCount(1))
                .WithGpu(gpu => gpu.WithDriver("nvidia"))
                .Build();
            ServiceDefinition all = new ServiceBuilder().WithGpu(gpu => gpu.WithCount(1)).WithAllGpus().Build();

            Assert.False(some.Gpus!.All);
            Assert.Equal(2, some.Gpus.Devices.Count);
            Assert.True(all.Gpus!.All);
            Assert.Empty(all.Gpus.Devices);
        }

        [Fact]
        public void Gpu_CountAndDeviceIds_AreExclusive()
        {
            Assert.Throws<InvalidOperationException>(() => new GpuBuilder().WithCount(1).WithDeviceId("g").Build());
            Assert.Throws<ArgumentOutOfRangeException>(() => new GpuBuilder().WithCount(0));
        }

        [Fact]
        public void Ulimit_NeedsASingleValueOrBothLimits()
        {
            Assert.True(new UlimitBuilder().WithLimit(5).Build().IsSingle);
            Assert.False(new UlimitBuilder().WithSoft(1).WithHard(2).Build().IsSingle);
            Assert.False(new UlimitBuilder().WithLimit(5).WithExtension("x-n", 1).Build().IsSingle);
            Assert.Throws<InvalidOperationException>(() => new UlimitBuilder().Build());
            Assert.Throws<InvalidOperationException>(() => new UlimitBuilder().WithSoft(1).Build());
            Assert.Throws<InvalidOperationException>(() => new UlimitBuilder().WithLimit(1).WithSoft(1).WithHard(2).Build());
        }

        [Fact]
        public void Ulimit_NameMustBeLowerCaseLetters_AndSameNameReplaces()
        {
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithUlimit("NoFile", 1));
            Assert.Throws<ArgumentException>(() => new ServiceBuilder().WithUlimit("no_file", 1));

            ServiceDefinition service = new ServiceBuilder().WithUlimit("nproc", 1).WithUlimit("nproc", 2).Build();

            Assert.Equal(2, service.Ulimits["nproc"].Single);
            Assert.Single(service.Ulimits);
        }

        [Fact]
        public void Blkio_Validates()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BlkioConfigBuilder().WithWeight(9));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BlkioWeightBuilder().WithWeight(1001));
            Assert.Throws<InvalidOperationException>(() => new BlkioLimitBuilder().WithPath("/dev/a").Build());
            Assert.Throws<InvalidOperationException>(() => new BlkioWeightBuilder().WithPath("/dev/a").Build());

            BlkioConfigDefinition config = new BlkioConfigBuilder()
                .WithWeightDevice("/dev/a", 500)
                .WithDeviceReadBps("/dev/a", "1mb")
                .WithDeviceReadIops("/dev/a", "10")
                .WithDeviceWriteBps("/dev/a", "2mb")
                .WithDeviceWriteIops("/dev/a", "20")
                .Build();

            Assert.Single(config.WeightDevice);
            Assert.Equal("1mb", config.DeviceReadBps[0].Rate);
            Assert.Equal("20", config.DeviceWriteIops[0].Rate);
        }
    }
}
