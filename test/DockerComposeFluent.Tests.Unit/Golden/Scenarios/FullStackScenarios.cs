using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>full-stack</c> golden scenarios: realistic multi-service applications that combine several features.
    /// </summary>
    internal static class FullStackScenarios
    {
        /// <summary>
        /// A web front end, an API, a database and a cache, split across a public and a private network.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile WebApp()
        {
            return new DockerComposeBuilder()
                .WithName("webapp")
                .WithService("proxy", service => service
                    .WithContainerName("webapp-proxy")
                    .WithImage("nginx:1.27")
                    .WithPort(80, 80)
                    .WithPort(port => port.WithTarget(443).WithPublished(443).WithHostIp("0.0.0.0").WithName("https"))
                    .WithVolume("./proxy/nginx.conf", "/etc/nginx/nginx.conf", readOnly: true)
                    .WithNetwork("public")
                    .WithNetwork("private", network => network.WithAlias("proxy.internal")))
                .WithService("api", service => service
                    .WithImage("ghcr.io/example/api:2.4.1")
                    .WithCommand(new[] { "dotnet", "Api.dll", "--urls", "http://+:8080" })
                    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
                    .WithEnvironment("ConnectionStrings__Db", "Host=db;Database=app;Username=app")
                    .WithEnvironment("Cache__Enabled", true)
                    .WithEnvironment("Cache__TimeoutSeconds", 30)
                    .WithEnvironment("Db__Password")
                    .WithVolume(mount => mount.WithType(MountType.Tmpfs).WithTarget("/tmp"))
                    .WithNetwork("private"))
                .WithService("db", service => service
                    .WithImage("postgres:17")
                    .WithEnvironment(new[] { "POSTGRES_DB=app", "POSTGRES_USER=app", "POSTGRES_PASSWORD" })
                    .WithVolume("db-data", "/var/lib/postgresql/data")
                    .WithNetwork("private", network => network.WithAlias("database")))
                .WithService("cache", service => service
                    .WithImage("redis:7")
                    .WithCommand("redis-server --save 60 1 --loglevel warning")
                    .WithVolume("cache-data:/data")
                    .WithNetwork("private"))
                .WithNetwork("public", network => network.WithDriver("bridge"))
                .WithNetwork("private", network => network.WithInternal(true))
                .WithVolume("db-data", volume => volume.WithDriver("local"))
                .WithVolume("cache-data", volume => volume.WithName("webapp_cache"))
                .Build();
        }
    }
}
