using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The annotation and lifecycle hook golden scenarios.
    /// </summary>
    internal static class ServiceHooksScenarios
    {
        /// <summary>
        /// Init-container steps run before the service starts: the example from the specification, then a step
        /// using every setting.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile PreStart()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("myapp:latest")
                    .WithDependsOn("db", DependencyCondition.ServiceHealthy)
                    .WithPreStart(step => step.WithCommand(new[] { "./manage.py", "migrate" }))
                    .WithPreStart(step => step
                        .WithImage("busybox")
                        .WithCommand("sh -c 'chown -R 1000:1000 /data'"))
                    .WithVolume("data", "/data"))
                .WithService("worker", service => service
                    .WithImage("myapp:latest")
                    .WithPreStart("./warm-cache.sh")
                    .WithPreStart(step => step
                        .WithImage("busybox")
                        .WithCommand(new[] { "sh", "-c", "echo ready" })
                        .WithUser("root")
                        .WithPrivileged(true)
                        .WithWorkingDir("/data")
                        .WithEnvironment("MODE", "init")
                        .WithPerReplica(true)
                        .WithExtension("x-note", "once per replica")))
                .WithService("db", service => service.WithImage("postgres:16").WithHealthcheck("pg_isready"))
                .WithVolume("data", volume => volume.WithDriver("local"))
                .Build();
        }

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
