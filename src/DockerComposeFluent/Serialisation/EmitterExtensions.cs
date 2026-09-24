using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Extension methods for emitting the YAML events shared by all converters.
    /// </summary>
    internal static class EmitterExtensions
    {
        /// <summary>
        /// Starts a block-style mapping.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        internal static void StartMapping(this IEmitter emitter)
        {
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));
        }

        /// <summary>
        /// Ends the current mapping.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        internal static void EndMapping(this IEmitter emitter)
        {
            emitter.Emit(new MappingEnd());
        }

        /// <summary>
        /// Starts a sequence.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="style">Block style (one item per line) or flow style (<c>[a, b]</c>).</param>
        internal static void StartSequence(this IEmitter emitter, SequenceStyle style)
        {
            emitter.Emit(new SequenceStart(null, null, true, style));
        }

        /// <summary>
        /// Ends the current sequence.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        internal static void EndSequence(this IEmitter emitter)
        {
            emitter.Emit(new SequenceEnd());
        }

        /// <summary>
        /// Writes a user-supplied string (a value or a name), always double-quoted so that no YAML reader
        /// can mistake it for a boolean, number, date or null.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="value">The string to write.</param>
        internal static void WriteScalar(this IEmitter emitter, string value)
        {
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value, ScalarStyle.DoubleQuoted, false, true));
        }

        /// <summary>
        /// Writes one of this library's own fixed mapping keys, such as <c>image</c>. These are never
        /// ambiguous, so they are written plain.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The key to write.</param>
        internal static void WriteKey(this IEmitter emitter, string key)
        {
            emitter.Emit(new Scalar(key));
        }

        /// <summary>
        /// Writes a fixed key and a string value that is always present.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="value">The value to write.</param>
        internal static void WriteScalarEntry(this IEmitter emitter, string key, string value)
        {
            emitter.WriteKey(key);
            emitter.WriteScalar(value);
        }

        /// <summary>
        /// Writes a fixed key and a string value, or nothing when <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="value">The value to write, or <c>null</c> to omit the entry.</param>
        internal static void WriteOptionalScalar(this IEmitter emitter, string key, string? value)
        {
            if (value == null)
            {
                return;
            }

            emitter.WriteKey(key);
            emitter.WriteScalar(value);
        }

        /// <summary>
        /// Writes a key and a value serialised by its own converter, or nothing when <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The key to write.</param>
        /// <param name="value">The value to write, or <c>null</c> to omit the entry.</param>
        /// <param name="serialiser">Serialises the value using the registered converters.</param>
        internal static void WriteOptionalValue<T>(this IEmitter emitter, string key, T? value, ObjectSerializer serialiser)
            where T : class
        {
            if (value == null)
            {
                return;
            }

            emitter.WriteKey(key);
            serialiser(value, typeof(T));
        }

        /// <summary>
        /// Writes a fixed key and an integer value as a plain (unquoted) number.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="value">The value to write.</param>
        internal static void WriteInteger(this IEmitter emitter, string key, int value)
        {
            emitter.WriteKey(key);
            emitter.Emit(new Scalar(value.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Writes a fixed key and a boolean value, or nothing when <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="value">The value to write, or <c>null</c> to omit the entry.</param>
        internal static void WriteOptionalBoolean(this IEmitter emitter, string key, bool? value)
        {
            if (value == null)
            {
                return;
            }

            emitter.WriteKey(key);
            emitter.Emit(new Scalar(value.Value ? "true" : "false"));
        }

        /// <summary>
        /// Writes a fixed key and an integer value, or nothing when <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="value">The value to write, or <c>null</c> to omit the entry.</param>
        internal static void WriteOptionalInteger(this IEmitter emitter, string key, int? value)
        {
            if (value == null)
            {
                return;
            }

            emitter.WriteInteger(key, value.Value);
        }

        /// <summary>
        /// Writes a fixed key and a block sequence of user-supplied strings, or nothing when
        /// <paramref name="values"/> is empty.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="values">The strings to write.</param>
        internal static void WriteOptionalStringSequence(this IEmitter emitter, string key, IReadOnlyList<string> values)
        {
            if (values.Count == 0)
            {
                return;
            }

            emitter.WriteKey(key);
            emitter.StartSequence(SequenceStyle.Block);

            foreach (string value in values)
            {
                emitter.WriteScalar(value);
            }

            emitter.EndSequence();
        }

        /// <summary>
        /// Writes a fixed key and a mapping of user-supplied names to strings, or nothing when
        /// <paramref name="values"/> is empty.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="values">The entries to write.</param>
        internal static void WriteOptionalStringMap(this IEmitter emitter, string key, IReadOnlyDictionary<string, string> values)
        {
            if (values.Count == 0)
            {
                return;
            }

            emitter.WriteKey(key);
            emitter.StartMapping();

            foreach (KeyValuePair<string, string> entry in values)
            {
                emitter.WriteScalar(entry.Key);
                emitter.WriteScalar(entry.Value);
            }

            emitter.EndMapping();
        }

        /// <summary>
        /// Writes a fixed key and a block sequence of values serialised by their own converters,
        /// or nothing when <paramref name="values"/> is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The fixed key to write.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="serialiser">Serialises each value using the registered converters.</param>
        internal static void WriteOptionalSequence<T>(this IEmitter emitter, string key, IReadOnlyList<T> values, ObjectSerializer serialiser)
        {
            if (values.Count == 0)
            {
                return;
            }

            emitter.WriteKey(key);
            emitter.StartSequence(SequenceStyle.Block);

            foreach (T value in values)
            {
                serialiser(value, typeof(T));
            }

            emitter.EndSequence();
        }

        /// <summary>
        /// Writes a fixed key and a mapping of user-named values, or nothing when <paramref name="values"/> is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the mapping.</typeparam>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The key to write.</param>
        /// <param name="values">The named values to write.</param>
        /// <param name="serialiser">Serialises each value using the registered converters.</param>
        internal static void WriteMap<T>(this IEmitter emitter, string key, IReadOnlyDictionary<string, T> values, ObjectSerializer serialiser)
        {
            if (values.Count == 0)
            {
                return;
            }

            emitter.WriteKey(key);
            emitter.StartMapping();

            foreach (KeyValuePair<string, T> entry in values)
            {
                emitter.WriteScalar(entry.Key);
                serialiser(entry.Value, typeof(T));
            }

            emitter.EndMapping();
        }
    }
}
