using System;
using System.Collections;
using System.Collections.Generic;

namespace DockerComposeFluent
{
    /// <summary>
    /// Structural equality for the values held by the models, so two models with the same content compare
    /// equal even when their lists and dictionaries are different instances.
    /// </summary>
    internal static class ValueEquality
    {
        internal static bool Equal(object? left, object? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            if (left is string || right is string)
            {
                return left.Equals(right);
            }

            if (left is IDictionary leftDictionary && right is IDictionary rightDictionary)
            {
                return SequenceEqual(Entries(leftDictionary), Entries(rightDictionary));
            }

            if (left is IEnumerable leftItems && right is IEnumerable rightItems)
            {
                return SequenceEqual(leftItems, rightItems);
            }

            return left.Equals(right);
        }

        internal static int Hash(object? value)
        {
            if (value == null)
            {
                return 0;
            }

            if (value is string)
            {
                return value.GetHashCode();
            }

            if (value is IDictionary dictionary)
            {
                return SequenceHash(Entries(dictionary));
            }

            if (value is IEnumerable items)
            {
                return SequenceHash(items);
            }

            return value.GetHashCode();
        }

        internal static bool SequenceEqual(IEnumerable left, IEnumerable right)
        {
            IEnumerator leftEnumerator = left.GetEnumerator();
            IEnumerator rightEnumerator = right.GetEnumerator();
            while (true)
            {
                bool hasLeft = leftEnumerator.MoveNext();
                bool hasRight = rightEnumerator.MoveNext();
                if (hasLeft != hasRight)
                {
                    return false;
                }

                if (!hasLeft)
                {
                    return true;
                }

                if (!Equal(leftEnumerator.Current, rightEnumerator.Current))
                {
                    return false;
                }
            }
        }

        internal static int SequenceHash(IEnumerable items)
        {
            int hash = 17;
            foreach (object? item in items)
            {
                hash = unchecked(hash * 31 + Hash(item));
            }

            return hash;
        }

        private static IEnumerable Entries(IDictionary dictionary)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                yield return new object?[] { entry.Key, entry.Value };
            }
        }
    }

    /// <summary>
    /// A read-only list that compares equal to any other list holding equal items in the same order.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    internal sealed class EquatableList<T> : IReadOnlyList<T>
    {
        internal static readonly EquatableList<T> Empty = new EquatableList<T>(Array.Empty<T>());

        private readonly T[] _items;

        internal EquatableList(IEnumerable<T> items)
        {
            _items = new List<T>(items).ToArray();
        }

        public int Count
        {
            get { return _items.Length; }
        }

        public T this[int index]
        {
            get { return _items[index]; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public override bool Equals(object? obj)
        {
            return obj is EquatableList<T> other && ValueEquality.SequenceEqual(_items, other._items);
        }

        public override int GetHashCode()
        {
            return ValueEquality.SequenceHash(_items);
        }
    }

    /// <summary>
    /// A read-only dictionary with string keys that compares equal to any other holding equal entries in the
    /// same order. The order is part of the comparison because it decides the order of the generated YAML.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    internal sealed class EquatableDictionary<TValue> : IReadOnlyDictionary<string, TValue>
    {
        internal static readonly EquatableDictionary<TValue> Empty =
            new EquatableDictionary<TValue>(new Dictionary<string, TValue>());

        private readonly Dictionary<string, TValue> _entries;

        internal EquatableDictionary(IEnumerable<KeyValuePair<string, TValue>> entries)
        {
            _entries = new Dictionary<string, TValue>();
            foreach (KeyValuePair<string, TValue> entry in entries)
            {
                _entries[entry.Key] = entry.Value;
            }
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public IEnumerable<string> Keys
        {
            get { return _entries.Keys; }
        }

        public IEnumerable<TValue> Values
        {
            get { return _entries.Values; }
        }

        public TValue this[string key]
        {
            get { return _entries[key]; }
        }

        public bool ContainsKey(string key)
        {
            return _entries.ContainsKey(key);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _entries.TryGetValue(key, out value!);
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is EquatableDictionary<TValue> other) || other._entries.Count != _entries.Count)
            {
                return false;
            }

            using (IEnumerator<KeyValuePair<string, TValue>> left = _entries.GetEnumerator())
            using (IEnumerator<KeyValuePair<string, TValue>> right = other._entries.GetEnumerator())
            {
                while (left.MoveNext() && right.MoveNext())
                {
                    if (left.Current.Key != right.Current.Key || !ValueEquality.Equal(left.Current.Value, right.Current.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (KeyValuePair<string, TValue> entry in _entries)
            {
                hash = unchecked(hash * 31 + entry.Key.GetHashCode());
                hash = unchecked(hash * 31 + ValueEquality.Hash(entry.Value));
            }

            return hash;
        }
    }
}
