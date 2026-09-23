using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    internal abstract class YamlConverter<T> : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(T);
        }

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserialiser)
        {
            throw new NotSupportedException("Reading compose files is not supported.");
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serialiser)
        {
            Write(emitter, (T)value!, serialiser);
        }

        protected abstract void Write(IEmitter emitter, T value, ObjectSerializer serialiser);
    }
}
