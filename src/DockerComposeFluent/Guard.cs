using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace DockerComposeFluent
{
    /// <summary>
    /// Argument validation shared by the builders.
    /// </summary>
    internal static class Guard
    {
        /// <summary>
        /// Throws if <paramref name="value"/> is <c>null</c>, empty or whitespace.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void NotNullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null, empty or whitespace.", parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void NotNull(object? value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not a compose-spec duration such as <c>30s</c> or <c>1m30s</c>.
        /// </summary>
        /// <param name="value">The duration text to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void Duration(string? value, string parameterName)
        {
            if (!Durations.IsValid(value))
            {
                throw new ArgumentException(
                    $"'{value}' is not a duration. Use whole numbers with the units us, ms, s, m or h, such as 30s or 1m30s.",
                    parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not a valid TCP or UDP port number (1 to 65535).
        /// </summary>
        /// <param name="value">The port number to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void ValidPort(int value, string parameterName)
        {
            if (value < 1 || value > 65535)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "A port must be between 1 and 65535.");
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not a valid IP address of the given family
        /// (a full dotted-quad for IPv4).
        /// </summary>
        /// <param name="value">The address to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        /// <param name="family">The required address family, or <c>null</c> to allow IPv4 or IPv6.</param>
        internal static void IpAddress(string? value, string parameterName, AddressFamily? family = null)
        {
            NotNullOrWhiteSpace(value, parameterName);

            if (!IsIpAddress(value!, family))
            {
                string kind = family == AddressFamily.InterNetwork ? "IPv4" : family == AddressFamily.InterNetworkV6 ? "IPv6" : "IP";
                throw new ArgumentException("'" + value + "' is not a valid " + kind + " address.", parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not a network in CIDR format, such as <c>172.28.0.0/16</c>.
        /// </summary>
        /// <param name="value">The CIDR value to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void Cidr(string? value, string parameterName)
        {
            NotNullOrWhiteSpace(value, parameterName);

            int slash = value!.IndexOf('/');
            if (slash > 0
                && int.TryParse(value.Substring(slash + 1), NumberStyles.None, CultureInfo.InvariantCulture, out int prefix)
                && TryParseAddress(value.Substring(0, slash), null, out IPAddress? address)
                && prefix <= (address!.AddressFamily == AddressFamily.InterNetwork ? 32 : 128))
            {
                return;
            }

            throw new ArgumentException("'" + value + "' is not a valid network in CIDR format, such as 172.28.0.0/16.", parameterName);
        }

        private static bool IsIpAddress(string value, AddressFamily? family)
        {
            return TryParseAddress(value, family, out _);
        }

        private static bool TryParseAddress(string value, AddressFamily? family, out IPAddress? address)
        {
            IPAddress? parsed;
            if (!IPAddress.TryParse(value, out parsed) || parsed == null)
            {
                address = null;
                return false;
            }

            bool matches;
            if (parsed.AddressFamily == AddressFamily.InterNetwork)
            {
                matches = family != AddressFamily.InterNetworkV6 && value.Split('.').Length == 4;
            }
            else
            {
                matches = parsed.AddressFamily == AddressFamily.InterNetworkV6 && family != AddressFamily.InterNetwork;
            }

            address = matches ? parsed : null;
            return matches;
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not usable as a label key: it must not be blank, and must not
        /// start with the reserved <c>com.docker.compose</c> prefix, which Compose sets itself and rejects at
        /// runtime if the file also sets it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels"/>
        /// </summary>
        /// <param name="value">The key to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void LabelKey(string? value, string parameterName)
        {
            NotNullOrWhiteSpace(value, parameterName);

            const string reservedPrefix = "com.docker.compose";
            if (value!.StartsWith(reservedPrefix, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"'{value}' starts with the reserved '{reservedPrefix}' label prefix, which Compose sets itself and rejects at runtime.",
                    parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not a valid compose-spec profile name: at least two characters,
        /// starting with a letter or digit, and otherwise letters, digits, <c>_</c>, <c>.</c> or <c>-</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#profiles"/>
        /// </summary>
        /// <param name="value">The profile name to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void ProfileName(string? value, string parameterName)
        {
            if (value == null || value.Length < 2 || !IsProfileNameStart(value[0]))
            {
                throw new ArgumentException(
                    $"'{value}' is not a valid profile name. Use at least two characters, starting with a letter or digit, and otherwise letters, digits, _, . or -.",
                    parameterName);
            }

            for (int i = 1; i < value.Length; i++)
            {
                if (!IsProfileNameStart(value[i]) && value[i] != '_' && value[i] != '.' && value[i] != '-')
                {
                    throw new ArgumentException(
                        $"'{value}' is not a valid profile name. Use at least two characters, starting with a letter or digit, and otherwise letters, digits, _, . or -.",
                        parameterName);
                }
            }
        }

        private static bool IsProfileNameStart(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not usable as an environment variable name: it must not be
        /// blank and must not contain <c>=</c>.
        /// </summary>
        /// <param name="value">The name to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void EnvironmentKey(string? value, string parameterName)
        {
            NotNullOrWhiteSpace(value, parameterName);

            if (value!.IndexOf('=') >= 0)
            {
                throw new ArgumentException("An environment variable name must not contain '='.", parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not a valid compose-spec pull policy: <c>always</c>,
        /// <c>never</c>, <c>build</c>, <c>if_not_present</c>, <c>missing</c>, <c>refresh</c>, <c>daily</c>,
        /// <c>weekly</c> or <c>every_&lt;duration&gt;</c>, such as <c>every_12h</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pull_policy"/>
        /// </summary>
        /// <param name="value">The policy to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void PullPolicy(string? value, string parameterName)
        {
            if (value == null || !System.Text.RegularExpressions.Regex.IsMatch(value, "^(always|never|build|if_not_present|missing|refresh|daily|weekly|every_([0-9]+[wdhms])+)$"))
            {
                throw new ArgumentException(
                    $"'{value}' is not a pull policy. Use always, never, build, if_not_present, missing, refresh, daily, weekly or every_<duration>.",
                    parameterName);
            }
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is not usable as an extension field key: it must not be blank
        /// and must start with the compose-spec <c>x-</c> prefix.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="value">The key to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        internal static void ExtensionKey(string? value, string parameterName)
        {
            NotNullOrWhiteSpace(value, parameterName);

            if (!value!.StartsWith("x-", StringComparison.Ordinal))
            {
                throw new ArgumentException($"'{value}' is not an extension field key. It must start with 'x-'.", parameterName);
            }
        }
    }
}
