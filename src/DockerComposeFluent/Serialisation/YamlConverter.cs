using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Base class for converters that write a single model type to YAML. Reading is not supported.
    /// Concrete converters are discovered and registered automatically by <see cref="ComposeYamlSerialiser"/>.
    /// </summary>
    /// <typeparam name="T">The model type this converter writes.</typeparam>
    internal abstract class YamlConverter<T> : IYamlTypeConverter
    {
        /// <inheritdoc />
        public bool Accepts(Type type)
        {
            return type == typeof(T);
        }

        /// <inheritdoc />
        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserialiser)
        {
            throw new NotSupportedException("Reading compose files is not supported.");
        }

        /// <inheritdoc />
        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serialiser)
        {
            Write(emitter, (T)value!, serialiser);
        }

        /// <summary>
        /// Writes <paramref name="value"/> to the YAML stream.
        /// </summary>
        /// <param name="emitter">The emitter to write YAML events to.</param>
        /// <param name="value">The model instance to write.</param>
        /// <param name="serialiser">Serialises nested values using the other registered converters.</param>
        protected abstract void Write(IEmitter emitter, T value, ObjectSerializer serialiser);
    }
}
