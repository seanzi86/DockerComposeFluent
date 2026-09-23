using System.Collections.Generic;
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
        /// Starts a flow-style sequence, for example <c>[a, b]</c>.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        internal static void StartSequence(this IEmitter emitter)
        {
            emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Flow));
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
        /// Writes a string scalar through YamlDotNet's serialiser, which quotes it when a plain scalar
        /// would be read back as something other than a string.
        /// </summary>
        /// <param name="emitter">The emitter being written to.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="serialiser">Serialises the string using the registered converters and quoting rules.</param>
        internal static void WriteScalar(this IEmitter emitter, string value, ObjectSerializer serialiser)
        {
            serialiser(value, typeof(string));
        }

        /// <summary>
        /// Writes a mapping key.
        /// </summary>
        /// <param name="emitter">The emitter being written to.</param>
        /// <param name="key">The key to write.</param>
        /// <param name="serialiser">Serialises the key using the registered converters and quoting rules.</param>
        internal static void WriteKey(this IEmitter emitter, string key, ObjectSerializer serialiser)
        {
            emitter.WriteScalar(key, serialiser);
        }

        /// <summary>
        /// Writes a key and scalar value, or nothing when <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="emitter">The emitter being written to.</param>
        /// <param name="key">The key to write.</param>
        /// <param name="value">The value to write, or <c>null</c> to omit the entry.</param>
        /// <param name="serialiser">Serialises the strings using the registered converters and quoting rules.</param>
        internal static void WriteOptionalScalar(this IEmitter emitter, string key, string? value, ObjectSerializer serialiser)
        {
            if (value == null)
            {
                return;
            }

            emitter.WriteKey(key, serialiser);
            emitter.WriteScalar(value, serialiser);
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

            emitter.WriteKey(key, serialiser);
            serialiser(value, typeof(T));
        }

        /// <summary>
        /// Writes a key and a mapping of named values, or nothing when <paramref name="values"/> is empty.
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

            emitter.WriteKey(key, serialiser);
            emitter.StartMapping();

            foreach (KeyValuePair<string, T> entry in values)
            {
                emitter.WriteKey(entry.Key, serialiser);
                serialiser(entry.Value, typeof(T));
            }

            emitter.EndMapping();
        }
    }
}
