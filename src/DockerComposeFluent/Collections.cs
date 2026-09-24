using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DockerComposeFluent
{
    /// <summary>
    /// Helpers for building new read-only collections from existing ones, so builders can add to the
    /// immutable collections held by the models without changing them in place.
    /// </summary>
    internal static class Collections
    {
        /// <summary>
        /// Gets a shared empty read-only dictionary, so that models with no entries compare as equal.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <returns>The shared empty dictionary.</returns>
        internal static IReadOnlyDictionary<string, TValue> EmptyDictionary<TValue>()
        {
            return EmptyDictionaryHolder<TValue>.Instance;
        }

        /// <summary>
        /// Creates a new read-only list containing the existing items followed by <paramref name="item"/>.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The existing items.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>A new read-only list.</returns>
        internal static IReadOnlyList<T> Append<T>(IReadOnlyList<T> items, T item)
        {
            List<T> copy = new List<T>(items) { item };
            return copy.AsReadOnly();
        }

        /// <summary>
        /// Creates a new dictionary with <paramref name="key"/> set to <paramref name="value"/>, replacing any
        /// existing entry and keeping the original order.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="entries">The existing entries.</param>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>A new dictionary.</returns>
        internal static IReadOnlyDictionary<string, TValue> With<TValue>(IReadOnlyDictionary<string, TValue> entries, string key, TValue value)
        {
            Dictionary<string, TValue> copy = new Dictionary<string, TValue>();
            foreach (KeyValuePair<string, TValue> entry in entries)
            {
                copy[entry.Key] = entry.Value;
            }

            copy[key] = value;
            return copy;
        }

        private static class EmptyDictionaryHolder<TValue>
        {
            internal static readonly IReadOnlyDictionary<string, TValue> Instance =
                new ReadOnlyDictionary<string, TValue>(new Dictionary<string, TValue>());
        }
    }
}
