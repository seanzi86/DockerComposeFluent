using System.Collections.Generic;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// The shared logic behind every <c>WithExtension</c> method, so the <c>x-</c> key rule is defined once.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
    /// </summary>
    internal static class ExtensionsMutator
    {
        /// <summary>
        /// Sets an extension field to a value. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="extensions">The current extension fields.</param>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is (a string, number, boolean, list, dictionary,
        /// or any other object YamlDotNet can serialise).</param>
        /// <param name="parameterName">The name of the key parameter being checked.</param>
        /// <returns>The updated extension fields.</returns>
        internal static IReadOnlyDictionary<string, object?> Set(IReadOnlyDictionary<string, object?> extensions, string key, object? value, string parameterName)
        {
            Guard.ExtensionKey(key, parameterName);

            return Collections.With(extensions, key, value);
        }
    }
}
