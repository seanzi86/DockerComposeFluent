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
    }
}
