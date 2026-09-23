using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Helpers for emitting the YAML events shared by all converters.
    /// </summary>
    internal static class YamlWriting
    {
        /// <summary>
        /// Starts a block-style mapping.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        internal static void StartMapping(IEmitter emitter)
        {
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));
        }

        /// <summary>
        /// Ends the current mapping.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        internal static void EndMapping(IEmitter emitter)
        {
            emitter.Emit(new MappingEnd());
        }

        /// <summary>
        /// Writes a mapping key.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The key to write.</param>
        internal static void WriteKey(IEmitter emitter, string key)
        {
            emitter.Emit(new Scalar(key));
        }

        /// <summary>
        /// Writes a key and scalar value, or nothing when <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The key to write.</param>
        /// <param name="value">The value to write, or <c>null</c> to omit the entry.</param>
        internal static void WriteOptionalScalar(IEmitter emitter, string key, string? value)
        {
            if (value == null)
            {
                return;
            }

            WriteKey(emitter, key);
            emitter.Emit(new Scalar(value));
        }

        /// <summary>
        /// Writes a key and a mapping of named values, or nothing when <paramref name="values"/> is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the mapping.</typeparam>
        /// <param name="emitter">The emitter to write to.</param>
        /// <param name="key">The key to write.</param>
        /// <param name="values">The named values to write.</param>
        /// <param name="serialiser">Serialises each value using the registered converters.</param>
        internal static void WriteMap<T>(IEmitter emitter, string key, IReadOnlyDictionary<string, T> values, ObjectSerializer serialiser)
        {
            if (values.Count == 0)
            {
                return;
            }

            WriteKey(emitter, key);
            StartMapping(emitter);

            foreach (KeyValuePair<string, T> entry in values)
            {
                WriteKey(emitter, entry.Key);
                serialiser(entry.Value, typeof(T));
            }

            EndMapping(emitter);
        }
    }
}
