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
        /// <returns>The validated value.</returns>
        internal static string NotNullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null, empty or whitespace.", parameterName);
            }

            return value!;
        }

        /// <summary>
        /// Throws if <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        /// <returns>The validated value.</returns>
        internal static T NotNull<T>(T? value, string parameterName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
