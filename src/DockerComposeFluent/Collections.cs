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
            return EquatableDictionary<TValue>.Empty;
        }

        /// <summary>
        /// Gets a shared empty read-only list.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <returns>The shared empty list.</returns>
        internal static IReadOnlyList<T> EmptyList<T>()
        {
            return EquatableList<T>.Empty;
        }

        /// <summary>
        /// Takes a private, value-comparable copy of a list, so models holding equal items compare equal and
        /// later changes to the caller's list cannot reach the model.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The list to copy.</param>
        /// <returns>The copy, or the same instance when it is already one.</returns>
        internal static IReadOnlyList<T> Seal<T>(IReadOnlyList<T> items)
        {
            return items == null || items is EquatableList<T> ? items! : new EquatableList<T>(items);
        }

        /// <summary>
        /// Takes a private, value-comparable copy of a dictionary.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="entries">The dictionary to copy.</param>
        /// <returns>The copy, or the same instance when it is already one.</returns>
        internal static IReadOnlyDictionary<string, TValue> Seal<TValue>(IReadOnlyDictionary<string, TValue> entries)
        {
            return entries == null || entries is EquatableDictionary<TValue> ? entries! : new EquatableDictionary<TValue>(entries);
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
    }
}
