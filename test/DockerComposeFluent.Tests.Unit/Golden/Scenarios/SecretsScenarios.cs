using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>secrets</c> golden scenarios.
    /// </summary>
    internal static class SecretsScenarios
    {
        /// <summary>
        /// A file-backed secret, an environment-backed secret, and an external secret with a custom name,
        /// referenced from services using the short form, the target-renaming helper, and the long form.
        /// </summary>
        /// <summary>
        /// The optional top-level secret settings: a secret driver with options, labels, and a template driver.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Settings()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service.WithImage("example/app").WithSecret("vault_secret"))
                .WithSecret("vault_secret", secret => secret
                    .WithEnvironment("VAULT_TOKEN")
                    .WithDriver("vault")
                    .WithDriverOption("path", "secret/data/app")
                    .WithLabel("com.example.rotation", "monthly")
                    .WithTemplateDriver("golang"))
                .Build();
        }

        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("frontend", service => service
                    .WithImage("example/webapp")
                    .WithSecret("server-certificate"))
                .WithService("api", service => service
                    .WithImage("example/api")
                    .WithSecret("token")
                    .WithSecret("server-certificate", "server.cert"))
                .WithService("admin", service => service
                    .WithImage("example/admin")
                    .WithSecret(secret => secret
                        .WithSource("server-certificate")
                        .WithTarget("server.cert")
                        .WithUid("103")
                        .WithGid("103")
                        .WithMode(Convert.ToInt32("440", 8))))
                .WithSecret("server-certificate", secret => secret.WithFile("./server.cert"))
                .WithSecret("token", secret => secret.WithEnvironment("OAUTH_TOKEN"))
                .WithSecret("external-cert", secret => secret.WithExternal(true).WithName("shared-cert"))
                .Build();
        }
    }
}
