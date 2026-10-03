using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The namespace and mode service setting golden scenarios.
    /// </summary>
    internal static class ServiceNamespacesScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("modes", service => service
                    .WithImage("example/app")
                    .WithNetworkMode("host")
                    .WithIpc("shareable")
                    .WithPid("host")
                    .WithUts("host")
                    .WithUsernsMode("host")
                    .WithCgroup(CgroupMode.Private)
                    .WithCgroupParent("m-executor-abcd"))
                .WithService("joined", service => service
                    .WithImage("example/sidecar")
                    .WithNetworkMode("service:modes")
                    .WithCgroup(CgroupMode.Host))
                .Build();
        }
    }
}
