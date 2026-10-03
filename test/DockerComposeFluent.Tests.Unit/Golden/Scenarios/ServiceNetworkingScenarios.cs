using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The DNS, host and linking service setting golden scenarios.
    /// </summary>
    internal static class ServiceNetworkingScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("db", service => service.WithImage("postgres"))
                .WithService("web", service => service
                    .WithImage("example/web")
                    .WithDns("8.8.8.8")
                    .WithDns(new[] { "9.9.9.9", "1.1.1.1" })
                    .WithDnsOpt("use-vc")
                    .WithDnsSearch(new[] { "dc1.example.com", "dc2.example.com" })
                    .WithExtraHost("somehost", "162.242.195.82")
                    .WithExtraHost("otherhost", "50.31.209.229")
                    .WithExtraHost("ipv6host", "2001:db8::1")
                    .WithExtraHost("host.docker.internal", "host-gateway")
                    .WithExpose(3000)
                    .WithExpose("8000-8010")
                    .WithLinks(new[] { "db", "db:database" })
                    .WithExternalLinks(new[] { "redis", "legacy:old" })
                    .WithVolumesFrom("db")
                    .WithVolumesFrom("db:ro")
                    .WithTmpfs("/run")
                    .WithTmpfs(new[] { "/tmp:size=64m", "/var/cache" }))
                .Build();
        }
    }
}
