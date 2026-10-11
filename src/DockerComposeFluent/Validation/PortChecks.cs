using System;
using System.Collections.Generic;
using System.Globalization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Checks that no two published ports bind the same host port, protocol and address.
    /// </summary>
    internal static class PortChecks
    {
        // The largest range expanded into individual ports, so a mistaken huge range cannot make this slow.
        private const int MaxRange = 4096;

        private sealed class Binding
        {
            internal Binding(string service, string path, string? hostIp, int port, string protocol)
            {
                Service = service;
                Path = path;
                HostIp = hostIp;
                Port = port;
                Protocol = protocol;
            }

            internal string Service { get; }

            internal string Path { get; }

            internal string? HostIp { get; }

            internal int Port { get; }

            internal string Protocol { get; }

            internal bool IsWildcard
            {
                get { return string.IsNullOrEmpty(HostIp) || HostIp == "0.0.0.0" || HostIp == "::"; }
            }
        }

        internal static void Run(DockerComposeFile file, ProblemCollector problems)
        {
            Dictionary<string, List<Binding>> byPort = new Dictionary<string, List<Binding>>();

            foreach (KeyValuePair<string, ServiceDefinition> entry in file.Services)
            {
                for (int i = 0; i < entry.Value.Ports.Count; i++)
                {
                    string path = "services." + entry.Key + ".ports[" + i + "]";

                    foreach (Binding binding in Parse(entry.Key, path, entry.Value.Ports[i]))
                    {
                        string key = binding.Port.ToString(CultureInfo.InvariantCulture) + "/" + binding.Protocol;
                        List<Binding>? existing;
                        if (!byPort.TryGetValue(key, out existing))
                        {
                            existing = new List<Binding>();
                            byPort[key] = existing;
                        }

                        Binding? clash = existing.Find(other => Overlaps(other, binding));
                        if (clash != null)
                        {
                            string message = clash.Service == binding.Service
                                ? $"Service \"{binding.Service}\" publishes host port {key} more than once (also at {clash.Path})."
                                : $"Service \"{binding.Service}\" publishes host port {key}, which service \"{clash.Service}\" also publishes ({clash.Path}).";

                            problems.Add(ValidationSeverity.Warning, ValidationCodes.DuplicatePublishedPort, binding.Path, message);
                        }

                        existing.Add(binding);
                    }
                }
            }
        }

        private static bool Overlaps(Binding first, Binding second)
        {
            return first.IsWildcard || second.IsWildcard || string.Equals(first.HostIp, second.HostIp, StringComparison.OrdinalIgnoreCase);
        }

        private static IEnumerable<Binding> Parse(string service, string path, PortMapping mapping)
        {
            string? hostIp;
            string? published;
            string protocol;

            if (mapping.Definition != null)
            {
                hostIp = mapping.Definition.HostIp;
                published = mapping.Definition.Published;
                protocol = mapping.Definition.Protocol == PortProtocol.Udp ? "udp" : "tcp";
            }
            else if (!TryParseShort(mapping.ShortSyntax!, out hostIp, out published, out protocol))
            {
                yield break;
            }

            foreach (int port in ExpandPorts(published))
            {
                yield return new Binding(service, path, hostIp, port, protocol);
            }
        }

        // "[ip:]published:container[/protocol]". A mapping with only a container port publishes nothing fixed.
        private static bool TryParseShort(string text, out string? hostIp, out string? published, out string protocol)
        {
            hostIp = null;
            published = null;
            protocol = "tcp";

            int slash = text.LastIndexOf('/');
            if (slash >= 0)
            {
                protocol = text.Substring(slash + 1).ToLowerInvariant();
                text = text.Substring(0, slash);
            }

            if (text.StartsWith("[", StringComparison.Ordinal))
            {
                int close = text.IndexOf(']');
                if (close < 0)
                {
                    return false;
                }

                hostIp = text.Substring(1, close - 1);
                text = text.Substring(close + 1).TrimStart(':');
                string[] rest = text.Split(':');
                published = rest.Length == 2 ? rest[0] : null;
                return published != null;
            }

            string[] parts = text.Split(':');
            if (parts.Length == 2)
            {
                published = parts[0];
            }
            else if (parts.Length == 3)
            {
                hostIp = parts[0];
                published = parts[1];
            }

            return published != null;
        }

        // A single port, or a range such as "8000-8010". Anything else (a variable, an empty value) is skipped.
        private static IEnumerable<int> ExpandPorts(string? published)
        {
            if (string.IsNullOrWhiteSpace(published))
            {
                yield break;
            }

            string[] range = published!.Split('-');
            int start;
            int end;

            if (range.Length == 1 && int.TryParse(range[0], NumberStyles.None, CultureInfo.InvariantCulture, out start))
            {
                yield return start;
            }
            else if (range.Length == 2
                && int.TryParse(range[0], NumberStyles.None, CultureInfo.InvariantCulture, out start)
                && int.TryParse(range[1], NumberStyles.None, CultureInfo.InvariantCulture, out end)
                && end >= start && end - start < MaxRange)
            {
                for (int port = start; port <= end; port++)
                {
                    yield return port;
                }
            }
        }
    }
}
