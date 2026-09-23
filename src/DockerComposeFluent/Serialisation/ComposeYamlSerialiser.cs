using System;
using System.Linq;
using DockerComposeFluent.Models;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    internal static class ComposeYamlSerialiser
    {
        private static readonly ISerializer Serialiser = CreateSerialiser();

        internal static string Serialise(DockerComposeFile file)
        {
            return Serialiser.Serialize(file);
        }

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
