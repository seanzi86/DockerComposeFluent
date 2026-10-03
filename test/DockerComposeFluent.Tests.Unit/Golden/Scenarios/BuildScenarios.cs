using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>build</c> golden scenarios.
    /// </summary>
    internal static class BuildScenarios
    {
        /// <summary>
        /// A context-only build (a plain string), a build that only sets a Dockerfile, and the raw-definition
        /// overload.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("short", service => service.WithBuild("./app"))
                .WithService("dockerfile", service => service
                    .WithBuild(build => build.WithContext("./app").WithDockerfile("Dockerfile.prod")))
                .WithService("inline", service => service
                    .WithBuild(build => build.WithDockerfileInline("FROM busybox\nRUN echo hello")))
                .WithService("raw", service => service
                    .WithBuild(new BuildDefinition { Context = "./raw", Target = "runtime" }))
                .Build();
        }

        /// <summary>
        /// Every setting of the long form, using each of the helper overloads.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Everything()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service
                    .WithImage("example/app:1.0")
                    .WithBuild(build => build
                        .WithContext("./app")
                        .WithDockerfile("Dockerfile")
                        .WithTarget("runtime")
                        .WithArg("GIT_COMMIT", "6d4f1e")
                        .WithArg("NODE_ENV", "production")
                        .WithAdditionalContext("base", "docker-image://alpine:3.20")
                        .WithLabel("com.example.stage", "release")
                        .WithCacheFrom("alpine:latest")
                        .WithCacheFrom("type=local,src=path/to/cache")
                        .WithCacheTo("type=local,dest=path/to/cache")
                        .WithEntitlements("network.host")
                        .WithExtraHost("somehost", "162.242.195.82")
                        .WithIsolation("default")
                        .WithNetwork("host")
                        .WithNoCache(true)
                        .WithNoCacheFilter("builder")
                        .WithPlatforms(new[] { "linux/amd64", "linux/arm64" })
                        .WithPrivileged(true)
                        .WithProvenance(true)
                        .WithPull(true)
                        .WithSbom("generator=docker/buildkit-syft-scanner")
                        .WithShmSize("128m")
                        .WithSsh("default")
                        .WithSsh("myproject=~/.ssh/myproject.pem")
                        .WithTags("example/app:latest")
                        .WithTags("registry.example.com/app:1.0")
                        .WithSecret("build_token")
                        .WithSecret(secret => secret.WithSource("npm_rc").WithTarget("/root/.npmrc"))
                        .WithUlimit("nproc", 65535)
                        .WithUlimit("nofile", 20000, 40000)
                        .WithExtension("x-note", "release build")))
                .WithSecret("build_token", secret => secret.WithEnvironment("BUILD_TOKEN"))
                .WithSecret("npm_rc", secret => secret.WithFile("./.npmrc"))
                .Build();
        }
    }
}
