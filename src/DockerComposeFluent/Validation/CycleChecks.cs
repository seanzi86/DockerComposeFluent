using System;
using System.Collections.Generic;
using System.Linq;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Checks that services do not depend on, or extend, each other in a cycle.
    /// </summary>
    internal static class CycleChecks
    {
        internal static void Run(DockerComposeFile file, ProblemCollector problems)
        {
            Dictionary<string, List<string>> dependsOn = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> extends = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, ServiceDefinition> entry in file.Services)
            {
                dependsOn[entry.Key] = entry.Value.DependsOn.Keys.Where(file.Services.ContainsKey).ToList();

                ExtendsDefinition? extend = entry.Value.Extends;
                extends[entry.Key] = extend != null && extend.File == null && extend.Service != null && file.Services.ContainsKey(extend.Service)
                    ? new List<string> { extend.Service }
                    : new List<string>();
            }

            foreach (IReadOnlyList<string> cycle in FindCycles(dependsOn))
            {
                problems.Add(
                    ValidationSeverity.Error,
                    ValidationCodes.DependencyCycle,
                    "services." + cycle[0] + ".depends_on." + cycle[1],
                    "Services depend on each other in a cycle: " + string.Join(" -> ", cycle) + ".");
            }

            foreach (IReadOnlyList<string> cycle in FindCycles(extends))
            {
                problems.Add(
                    ValidationSeverity.Error,
                    ValidationCodes.DependencyCycle,
                    "services." + cycle[0] + ".extends",
                    "Services extend each other in a cycle: " + string.Join(" -> ", cycle) + ".");
            }
        }

        // Each distinct cycle once, written starting from its alphabetically first service and ending on it
        // again, for example [a, b, a]. A service that points at itself is a cycle of one: [a, a].
        private static IEnumerable<IReadOnlyList<string>> FindCycles(IReadOnlyDictionary<string, List<string>> edges)
        {
            List<IReadOnlyList<string>> cycles = new List<IReadOnlyList<string>>();
            HashSet<string> seen = new HashSet<string>();
            Dictionary<string, int> state = new Dictionary<string, int>();
            List<string> stack = new List<string>();

            void Visit(string node)
            {
                state[node] = 1;
                stack.Add(node);

                foreach (string next in edges[node])
                {
                    int nextState;
                    state.TryGetValue(next, out nextState);

                    if (nextState == 0)
                    {
                        Visit(next);
                    }
                    else if (nextState == 1)
                    {
                        List<string> loop = stack.Skip(stack.IndexOf(next)).ToList();
                        int first = loop.IndexOf(loop.OrderBy(name => name, StringComparer.Ordinal).First());
                        List<string> rotated = loop.Skip(first).Concat(loop.Take(first)).ToList();
                        rotated.Add(rotated[0]);

                        if (seen.Add(string.Join("\n", rotated)))
                        {
                            cycles.Add(rotated);
                        }
                    }
                }

                stack.RemoveAt(stack.Count - 1);
                state[node] = 2;
            }

            foreach (string node in edges.Keys)
            {
                int nodeState;
                state.TryGetValue(node, out nodeState);
                if (nodeState == 0)
                {
                    Visit(node);
                }
            }

            return cycles;
        }
    }
}
