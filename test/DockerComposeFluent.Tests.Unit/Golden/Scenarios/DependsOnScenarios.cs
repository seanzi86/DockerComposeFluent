using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>depends_on</c> golden scenarios.
    /// </summary>
    internal static class DependsOnScenarios
    {
        /// <summary>
        /// Plain dependencies are written as a list of service names.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile ShortSyntax()
        {
            return new DockerComposeBuilder()
                .WithService("web", service => service
                    .WithImage("nginx")
                    .WithDependsOn("db")
                    .WithDependsOn(new[] { "redis", "queue" }))
                .WithService("db", service => service.WithImage("postgres"))
                .WithService("redis", service => service.WithImage("redis"))
                .WithService("queue", service => service.WithImage("rabbitmq"))
                .Build();
        }

        /// <summary>
        /// Every condition and setting. Any dependency with settings makes the whole set a mapping, and the
        /// condition is always written because Compose requires it.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile LongSyntax()
        {
            return new DockerComposeBuilder()
                .WithService("web", service => service
                    .WithImage("nginx")
                    .WithDependsOn("db", DependencyCondition.ServiceHealthy)
                    .WithDependsOn("migrate", DependencyCondition.ServiceCompletedSuccessfully)
                    .WithDependsOn("cache", DependencyCondition.ServiceStarted)
                    .WithDependsOn("proxy")
                    .WithDependsOn("metrics", dependency => dependency.WithRequired(false))
                    .WithDependsOn("config", dependency => dependency
                        .WithCondition(DependencyCondition.ServiceStarted)
                        .WithRestart(true)
                        .WithRequired(true)))
                .WithService("db", service => service.WithImage("postgres"))
                .WithService("migrate", service => service.WithImage("example/migrate"))
                .WithService("cache", service => service.WithImage("redis"))
                .WithService("proxy", service => service.WithImage("traefik"))
                .WithService("metrics", service => service.WithImage("prom/prometheus"))
                .WithService("config", service => service.WithImage("example/config"))
                .Build();
        }
    }
}
