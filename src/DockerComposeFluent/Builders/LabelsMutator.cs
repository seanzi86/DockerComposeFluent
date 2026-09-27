using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// The shared logic behind <c>WithLabel</c>/<c>WithLabels</c> on <see cref="ServiceBuilder"/>,
    /// <see cref="NetworkBuilder"/> and <see cref="VolumeBuilder"/>, so the map/list rule is defined once.
    /// </summary>
    internal static class LabelsMutator
    {
        /// <summary>
        /// Sets a label to a value, keeping the mapping form. Setting the same key again replaces its value.
        /// </summary>
        /// <param name="labels">The current labels.</param>
        /// <param name="key">The label key.</param>
        /// <param name="value">The label value.</param>
        /// <param name="parameterName">The name of the key parameter being checked.</param>
        /// <returns>The updated labels.</returns>
        internal static Labels Set(Labels labels, string key, string value, string parameterName)
        {
            Guard.LabelKey(key, parameterName);
            Guard.NotNull(value, nameof(value));

            return labels with { Values = Collections.With(labels.Values, key, value) };
        }

        /// <summary>
        /// Sets several labels from <c>KEY=value</c> strings, or a bare <c>KEY</c> for an empty value. Makes the
        /// YAML use the list form.
        /// </summary>
        /// <param name="labels">The current labels.</param>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <param name="parameterName">The name of the entries parameter being checked.</param>
        /// <returns>The updated labels.</returns>
        internal static Labels SetFromEntries(Labels labels, IEnumerable<string> entries, string parameterName)
        {
            Guard.NotNull(entries, nameof(entries));

            Labels result = labels with { UsesListSyntax = true };
            foreach (string entry in entries)
            {
                Guard.NotNullOrWhiteSpace(entry, parameterName);

                int separator = entry.IndexOf('=');
                string key = separator < 0 ? entry : entry.Substring(0, separator);
                string value = separator < 0 ? string.Empty : entry.Substring(separator + 1);
                result = Set(result, key, value, parameterName) with { UsesListSyntax = true };
            }

            return result;
        }
    }
}
