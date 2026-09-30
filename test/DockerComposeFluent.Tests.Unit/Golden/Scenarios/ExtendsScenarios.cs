using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>extends</c> golden scenarios.
    /// </summary>
    internal static class ExtendsScenarios
    {
        /// <summary>
        /// Extending a service in the same file (short form), a service in another file, and the raw
        /// definition and builder overloads.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("common", service => service
                    .WithImage("busybox")
                    .WithEnvironment("TZ", "utc"))
                .WithService("cli", service => service
                    .WithExtends("common")
                    .WithEnvironment("PORT", "80"))
                .WithService("remote", service => service
                    .WithExtends("webapp", "common.yml"))
                .WithService("definition", service => service
                    .WithExtends(new ExtendsDefinition { Service = "common" }))
                .WithService("builder", service => service
                    .WithExtends(extends => extends
                        .WithService("webapp")
                        .WithFile("common.yml")))
                .Build();
        }
    }
}
