using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Checks that a <c>service_healthy</c> dependency points at a service that can report health.
    /// </summary>
    internal static class HealthcheckChecks
    {
        internal static void Run(DockerComposeFile file, ProblemCollector problems)
        {
            foreach (KeyValuePair<string, ServiceDefinition> entry in file.Services)
            {
                foreach (KeyValuePair<string, DependencyDefinition> dependency in entry.Value.DependsOn)
                {
                    ServiceDefinition? target;
                    if (dependency.Value.Condition != DependencyCondition.ServiceHealthy || !file.Services.TryGetValue(dependency.Key, out target))
                    {
                        continue;
                    }

                    if (target.Healthcheck == null || target.Healthcheck.Disable == true)
                    {
                        problems.Add(
                            ValidationSeverity.Warning,
                            ValidationCodes.MissingHealthcheck,
                            "services." + entry.Key + ".depends_on." + dependency.Key,
                            $"Service \"{entry.Key}\" waits for \"{dependency.Key}\" to be healthy, but \"{dependency.Key}\" has no healthcheck, so the condition can never be met.");
                    }
                }
            }
        }
    }
}
