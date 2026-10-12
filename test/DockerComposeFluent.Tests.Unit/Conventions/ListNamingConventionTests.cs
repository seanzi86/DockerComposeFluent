using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DockerComposeFluent.Builders;

namespace DockerComposeFluent.Tests.Unit.Conventions
{
    // The rule (see CONTRIBUTING.md): a list of plain strings has ONE method named after the compose key, taking
    // a string or an IEnumerable<string>. A singular WithX(string) beside a plural WithXs(IEnumerable<string>) is
    // only for items that have several shapes (a short string, a long form, a builder lambda).
    public class ListNamingConventionTests
    {
        // Items with several shapes, where the singular method is a family of overloads and the plural a bulk add.
        private static readonly HashSet<string> _shapedItems = new HashSet<string>
        {
            "ServiceBuilder.WithPorts",
            "ServiceBuilder.WithVolumes",
            "ServiceBuilder.WithSecrets",
            "ServiceBuilder.WithConfigs",
            "ServiceBuilder.WithNetworks",
            "ServiceBuilder.WithEnvFiles",
            "DockerComposeBuilder.WithIncludes",
        };

        private static IEnumerable<MethodInfo> BuilderMethods()
        {
            return typeof(ServiceBuilder).Assembly.GetExportedTypes()
                .Where(type => type.IsClass && type.Name.EndsWith("Builder", StringComparison.Ordinal))
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(method => method.Name.StartsWith("With", StringComparison.Ordinal));
        }

        private static bool TakesOnly(MethodInfo method, Type parameter)
        {
            ParameterInfo[] parameters = method.GetParameters();
            return parameters.Length == 1 && parameters[0].ParameterType == parameter;
        }

        [Fact]
        public void PlainStringLists_HaveOneMethodNamedAfterTheKey_NotASingularPluralPair()
        {
            List<MethodInfo> methods = BuilderMethods().ToList();
            List<string> offenders = new List<string>();

            foreach (MethodInfo bulk in methods.Where(method => TakesOnly(method, typeof(IEnumerable<string>))))
            {
                string key = bulk.DeclaringType!.Name + "." + bulk.Name;
                if (_shapedItems.Contains(key))
                {
                    continue;
                }

                foreach (string singular in SingularNames(bulk.Name))
                {
                    if (methods.Any(method => method.DeclaringType == bulk.DeclaringType && method.Name == singular && TakesOnly(method, typeof(string))))
                    {
                        offenders.Add(key + " is paired with " + singular + "(string); give " + bulk.Name + " a string overload instead");
                    }
                }
            }

            Assert.True(offenders.Count == 0, string.Join(Environment.NewLine, offenders));
        }

        [Fact]
        public void EveryAllowListedPair_StillExists()
        {
            // A stale allow-list entry would silently excuse a future violation.
            List<MethodInfo> methods = BuilderMethods().ToList();

            foreach (string entry in _shapedItems)
            {
                string[] parts = entry.Split('.');
                Assert.True(
                    methods.Any(method => method.DeclaringType!.Name == parts[0] && method.Name == parts[1] && TakesOnly(method, typeof(IEnumerable<string>))),
                    entry + " is on the allow-list but no longer exists.");
            }
        }

        private static IEnumerable<string> SingularNames(string plural)
        {
            if (plural.EndsWith("ies", StringComparison.Ordinal))
            {
                yield return plural.Substring(0, plural.Length - 3) + "y";
            }

            if (plural.EndsWith("es", StringComparison.Ordinal))
            {
                yield return plural.Substring(0, plural.Length - 2);
            }

            if (plural.EndsWith("s", StringComparison.Ordinal))
            {
                yield return plural.Substring(0, plural.Length - 1);
            }
        }
    }
}
