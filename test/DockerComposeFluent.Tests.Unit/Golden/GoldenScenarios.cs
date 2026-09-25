using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    /// <summary>
    /// The catalogue of compose files whose YAML is checked against fixtures. Scenarios are discovered by
    /// reflection: to add one, add a <see cref="ScenarioAttribute"/> method to a <c>*Scenarios</c> class in
    /// <c>Golden/Scenarios</c> (or a new class for a new feature) and accept its Verify fixture (see CONTRIBUTING.md).
    /// </summary>
    internal static class GoldenScenarios
    {
        private const string _classSuffix = "Scenarios";

        /// <summary>
        /// Every scenario, keyed by its id (<c>&lt;feature&gt;/&lt;name&gt;</c>, for example <c>ports/forms</c>).
        /// </summary>
        internal static IReadOnlyDictionary<string, Func<DockerComposeFile>> All { get; } = Discover();

        private static IReadOnlyDictionary<string, Func<DockerComposeFile>> Discover()
        {
            SortedDictionary<string, Func<DockerComposeFile>> scenarios = new SortedDictionary<string, Func<DockerComposeFile>>(StringComparer.Ordinal);

            IEnumerable<Type> classes = typeof(GoldenScenarios).Assembly.GetTypes()
                .Where(type => type.IsClass && type.IsAbstract && type.IsSealed && type.Name.EndsWith(_classSuffix, StringComparison.Ordinal));

            foreach (Type type in classes)
            {
                string feature = Kebab(type.Name.Substring(0, type.Name.Length - _classSuffix.Length));
                IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(method => method.GetCustomAttribute<ScenarioAttribute>() != null);

                foreach (MethodInfo method in methods)
                {
                    if (method.ReturnType != typeof(DockerComposeFile) || method.GetParameters().Length != 0)
                    {
                        throw new InvalidOperationException($"{type.Name}.{method.Name} must be a static method with no parameters that returns a DockerComposeFile.");
                    }

                    MethodInfo captured = method;
                    scenarios.Add(feature + "/" + Kebab(method.Name), () => (DockerComposeFile)captured.Invoke(null, null)!);
                }
            }

            return scenarios;
        }

        private static string Kebab(string name)
        {
            return Regex.Replace(name, "(?<=[a-z0-9])(?=[A-Z])", "-").ToLowerInvariant();
        }
    }
}
