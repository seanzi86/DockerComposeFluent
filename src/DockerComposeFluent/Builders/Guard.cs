using System;

namespace DockerComposeFluent.Builders
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
    }
}
