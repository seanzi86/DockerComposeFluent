using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    internal static class YamlWriting
    {
        internal static void StartMapping(IEmitter emitter)
        {
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));
        }

        internal static void EndMapping(IEmitter emitter)
        {
            emitter.Emit(new MappingEnd());
        }

        internal static void WriteKey(IEmitter emitter, string key)
        {
            emitter.Emit(new Scalar(key));
        }

        internal static void WriteOptionalScalar(IEmitter emitter, string key, string? value)
        {
            if (value == null)
            {
                return;
            }

            WriteKey(emitter, key);
            emitter.Emit(new Scalar(value));
        }

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
