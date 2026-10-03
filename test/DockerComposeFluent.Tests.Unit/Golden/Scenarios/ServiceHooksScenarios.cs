using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The annotation and lifecycle hook golden scenarios.
    /// </summary>
    internal static class ServiceHooksScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("example/app")
                    .WithAnnotation("com.example.team", "platform")
                    .WithAnnotation("com.example.tier", "backend")
                    .WithPostStart("./migrate.sh")
                    .WithPostStart(hook => hook
                        .WithCommand(new[] { "sh", "-c", "echo started" })
                        .WithUser("root")
                        .WithPrivileged(true)
                        .WithWorkingDir("/app")
                        .WithEnvironment("MODE", "warm")
                        .WithExtension("x-note", "runs once"))
                    .WithPreStop("./drain.sh")
                    .WithPreStop(hook => hook.WithCommand(new[] { "sh", "-c", "sleep 5" })))
                .Build();
        }
    }
}
