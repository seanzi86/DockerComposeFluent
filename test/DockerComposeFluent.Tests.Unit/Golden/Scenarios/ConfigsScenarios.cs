using System;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>configs</c> golden scenarios.
    /// </summary>
    internal static class ConfigsScenarios
    {
        /// <summary>
        /// A file-backed config, an external config, and an inline-content config, referenced from services
        /// using the short form, the target-renaming helper, and the long form.
        /// </summary>
        /// <summary>
        /// The optional top-level config settings: labels (in the list form) and a template driver.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Settings()
        {
            return new DockerComposeBuilder()
                .WithService("app", service => service.WithImage("example/app").WithConfig("templated"))
                .WithConfig("templated", config => config
                    .WithContent("name={{ .Name }}")
                    .WithLabels(new[] { "com.example.owner=platform", "com.example.reviewed" })
                    .WithTemplateDriver("golang"))
                .Build();
        }

        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("redis", service => service
                    .WithImage("redis:latest")
                    .WithConfig("my_config")
                    .WithConfig("my_other_config"))
                .WithService("app", service => service
                    .WithImage("example/app")
                    .WithConfig("app_config")
                    .WithConfig("my_config", "/redis_config"))
                .WithService("admin", service => service
                    .WithImage("example/admin")
                    .WithConfig(config => config
                        .WithSource("my_config")
                        .WithTarget("/redis_config")
                        .WithUid("103")
                        .WithGid("103")
                        .WithMode(Convert.ToInt32("440", 8))))
                .WithConfig("my_config", config => config.WithFile("./my_config.txt"))
                .WithConfig("my_other_config", config => config.WithExternal(true))
                .WithConfig("app_config", config => config.WithContent("debug=true\nname=demo"))
                .Build();
        }
    }
}
