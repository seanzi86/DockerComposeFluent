using System;

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
    }
}
