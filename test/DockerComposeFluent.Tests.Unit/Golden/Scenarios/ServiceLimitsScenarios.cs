using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The CPU, memory, process and block I/O limit service setting golden scenarios.
    /// </summary>
    internal static class ServiceLimitsScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("limited", service => service
                    .WithImage("example/app")
                    .WithCpus(0.5)
                    .WithCpuCount(2)
                    .WithCpuPercent(50)
                    .WithCpuShares(512)
                    .WithCpuQuota(50000)
                    .WithCpuPeriod(100000)
                    .WithCpuRtPeriod(1000000)
                    .WithCpuRtRuntime(950000)
                    .WithCpuset("0-1")
                    .WithMemLimit("512m")
                    .WithMemReservation("256m")
                    .WithMemswapLimit("1g")
                    .WithMemSwappiness(10)
                    .WithOomKillDisable(true)
                    .WithOomScoreAdj(-500)
                    .WithPidsLimit(-1)
                    .WithShmSize("128m")
                    .WithSysctl("net.core.somaxconn", 1024)
                    .WithSysctl("net.ipv4.tcp_syncookies", "0")
                    .WithStorageOpt("size", "20G")
                    .WithUlimit("nproc", 65535)
                    .WithUlimit("nofile", 20000, 40000)
                    .WithUlimit("core", ulimit => ulimit.WithSoft(0).WithHard(0).WithExtension("x-note", "no core dumps"))
                    .WithBlkioConfig(blkio => blkio
                        .WithWeight(300)
                        .WithWeightDevice("/dev/sda", 400)
                        .WithDeviceReadBps("/dev/sdb", "12mb")
                        .WithDeviceReadIops("/dev/sdb", "120")
                        .WithDeviceWriteBps("/dev/sdb", "1gb")
                        .WithDeviceWriteIops("/dev/sdb", "30")))
                .Build();
        }
    }
}
