using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// Checks that every name a service refers to is defined: services, networks, named volumes, secrets and
    /// configs.
    /// </summary>
    internal static class ReferenceChecks
    {
        private const string IncludedNote = " It may be defined in an included file.";

        internal static void Run(DockerComposeFile file, ProblemCollector problems)
        {
            // A name that is not defined here may still come from an included file, so it is only a warning.
            bool mayBeIncluded = file.Includes.Count > 0;
            ValidationSeverity severity = mayBeIncluded ? ValidationSeverity.Warning : ValidationSeverity.Error;

            void Undefined(string code, string path, string message)
            {
                problems.Add(severity, code, path, mayBeIncluded ? message + IncludedNote : message);
            }

            foreach (KeyValuePair<string, ServiceDefinition> entry in file.Services)
            {
                string name = entry.Key;
                ServiceDefinition service = entry.Value;
                string root = "services." + name;

                void Service(string path, string target, string verb)
                {
                    if (IsKnown(target, file.Services))
                    {
                        return;
                    }

                    Undefined(ValidationCodes.UndefinedService, path, $"Service \"{name}\" {verb} service \"{target}\", which is not defined.");
                }

                foreach (string dependency in service.DependsOn.Keys)
                {
                    Service(root + ".depends_on." + dependency, dependency, "depends on");
                }

                for (int i = 0; i < service.Links.Count; i++)
                {
                    Service(root + ".links[" + i + "]", Before(service.Links[i], ':'), "links to");
                }

                for (int i = 0; i < service.VolumesFrom.Count; i++)
                {
                    string source = service.VolumesFrom[i];
                    if (!source.StartsWith("container:", StringComparison.Ordinal))
                    {
                        Service(root + ".volumes_from[" + i + "]", Before(source, ':'), "takes its volumes from");
                    }
                }

                ServiceReference(service.NetworkMode, root + ".network_mode", "uses the network of", Service);
                ServiceReference(service.Ipc, root + ".ipc", "shares the IPC namespace of", Service);
                ServiceReference(service.Pid, root + ".pid", "shares the PID namespace of", Service);

                // An extends with a file refers to a service in another file, which cannot be checked here.
                if (service.Extends != null && service.Extends.File == null && service.Extends.Service != null)
                {
                    Service(root + ".extends", service.Extends.Service, "extends");
                }

                foreach (string network in service.Networks.Keys)
                {
                    if (network != "default" && !IsKnown(network, file.Networks))
                    {
                        Undefined(ValidationCodes.UndefinedNetwork, root + ".networks." + network, $"Service \"{name}\" uses network \"{network}\", which is not defined in the top-level networks.");
                    }
                }

                for (int i = 0; i < service.Volumes.Count; i++)
                {
                    string? volume = NamedVolume(service.Volumes[i]);
                    if (volume != null && !IsKnown(volume, file.Volumes))
                    {
                        Undefined(ValidationCodes.UndefinedVolume, root + ".volumes[" + i + "]", $"Service \"{name}\" mounts volume \"{volume}\", which is not defined in the top-level volumes.");
                    }
                }

                CheckSecrets(file, name, root + ".secrets", service.Secrets, Undefined);
                if (service.Build != null)
                {
                    CheckSecrets(file, name, root + ".build.secrets", service.Build.Secrets, Undefined);
                }

                for (int i = 0; i < service.Configs.Count; i++)
                {
                    string? config = service.Configs[i].Source;
                    if (config != null && !IsKnown(config, file.Configs))
                    {
                        Undefined(ValidationCodes.UndefinedConfig, root + ".configs[" + i + "]", $"Service \"{name}\" uses config \"{config}\", which is not defined in the top-level configs.");
                    }
                }
            }
        }

        private static void CheckSecrets(DockerComposeFile file, string service, string path, IReadOnlyList<SecretReference> secrets, Action<string, string, string> undefined)
        {
            for (int i = 0; i < secrets.Count; i++)
            {
                string? secret = secrets[i].Source;
                if (secret != null && !IsKnown(secret, file.Secrets))
                {
                    undefined(ValidationCodes.UndefinedSecret, path + "[" + i + "]", $"Service \"{service}\" uses secret \"{secret}\", which is not defined in the top-level secrets.");
                }
            }
        }

        // network_mode, ipc and pid refer to another service only in the form "service:<name>".
        private static void ServiceReference(string? value, string path, string verb, Action<string, string, string> report)
        {
            const string prefix = "service:";
            if (value != null && value.StartsWith(prefix, StringComparison.Ordinal))
            {
                report(path, value.Substring(prefix.Length), verb);
            }
        }

        // A name containing "$" is a variable that Compose fills in later, so it cannot be checked.
        private static bool IsKnown<T>(string name, IReadOnlyDictionary<string, T> defined)
        {
            return name.IndexOf('$') >= 0 || defined.ContainsKey(name);
        }

        private static string Before(string text, char separator)
        {
            int index = text.IndexOf(separator);
            return index < 0 ? text : text.Substring(0, index);
        }

        // The named volume a mount refers to, or null for a bind mount, an anonymous volume or any other kind.
        private static string? NamedVolume(MountMapping mount)
        {
            if (mount.Definition != null)
            {
                return mount.Definition.Type == MountType.Volume ? mount.Definition.Source : null;
            }

            string text = mount.ShortSyntax!;
            int colon = text.IndexOf(':');
            if (colon <= 0)
            {
                return null;
            }

            string source = text.Substring(0, colon);
            bool windowsDrive = source.Length == 1 && char.IsLetter(source[0]) && colon + 1 < text.Length && (text[colon + 1] == '\\' || text[colon + 1] == '/');
            bool path = source[0] == '.' || source[0] == '/' || source[0] == '~';

            return windowsDrive || path ? null : source;
        }
    }
}
