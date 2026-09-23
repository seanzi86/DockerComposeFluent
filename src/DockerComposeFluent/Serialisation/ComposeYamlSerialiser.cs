using System;
using System.Linq;
using DockerComposeFluent.Models;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Serialises compose models to YAML using every <see cref="YamlDotNet.Serialization.IYamlTypeConverter"/>
    /// found in this assembly.
    /// </summary>
    internal static class ComposeYamlSerialiser
    {
        private static readonly ISerializer Serialiser = CreateSerialiser();

        /// <summary>
        /// Serialises a compose file to a YAML string.
        /// </summary>
        /// <param name="file">The compose file to serialise.</param>
        /// <returns>The YAML representation of <paramref name="file"/>.</returns>
        internal static string Serialise(DockerComposeFile file)
        {
            return Serialiser.Serialize(file);
        }

        /// <summary>
        /// Builds a serialiser with all converters in this assembly registered.
        /// </summary>
        private static ISerializer CreateSerialiser()
        {
            SerializerBuilder builder = new SerializerBuilder();

            Type[] converterTypes = typeof(ComposeYamlSerialiser).Assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IYamlTypeConverter).IsAssignableFrom(type))
                .ToArray();

            foreach (Type converterType in converterTypes)
            {
                builder.WithTypeConverter((IYamlTypeConverter)Activator.CreateInstance(converterType)!);
            }

            return builder.Build();
        }
    }
}
