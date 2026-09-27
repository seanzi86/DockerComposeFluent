using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>labels</c> and <c>label_file</c> golden scenarios.
    /// </summary>
    internal static class LabelsScenarios
    {
        /// <summary>
        /// Labels on a service, a network and a volume, in map form, list form, and bulk from a dictionary, plus
        /// <c>label_file</c> with a single and several paths.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("map", service => service
                    .WithImage("nginx")
                    .WithLabel("com.example.description", "Accounting webapp")
                    .WithLabel("com.example.label-with-empty-value", ""))
                .WithService("list", service => service
                    .WithImage("nginx")
                    .WithLabels(new[] { "com.example.description=Accounting webapp", "com.example.label-with-empty-value" }))
                .WithService("bulk", service => service
                    .WithImage("nginx")
                    .WithLabels(new Dictionary<string, string> { ["com.example.department"] = "Finance" }))
                .WithService("single-file", service => service.WithImage("nginx").WithLabelFile("./app.labels"))
                .WithService("multiple-files", service => service.WithImage("nginx").WithLabelFiles(new[] { "./app.labels", "./additional.labels" }))
                .WithNetwork("front", network => network.WithLabel("com.example.description", "Financial transaction network"))
                .WithVolume("db-data", volume => volume.WithLabel("com.example.description", "Database volume"))
                .Build();
        }
    }
}
