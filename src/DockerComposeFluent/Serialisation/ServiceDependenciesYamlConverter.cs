using System.Collections.Generic;
using System.Linq;
using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes the dependencies of a service as a list of names when no dependency has settings, or as a mapping
    /// of names to settings otherwise, since Compose does not allow the two forms to be mixed.
    /// </summary>
    internal sealed class ServiceDependenciesYamlConverter : YamlConverter<IReadOnlyDictionary<string, DependencyDefinition>>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, IReadOnlyDictionary<string, DependencyDefinition> value, ObjectSerializer serialiser)
        {
            if (value.Values.All(dependency => dependency.IsEmpty))
            {
                emitter.StartSequence(SequenceStyle.Block);
                foreach (string name in value.Keys)
                {
                    emitter.WriteScalar(name);
                }

                emitter.EndSequence();
                return;
            }

            emitter.StartMapping();
            foreach (KeyValuePair<string, DependencyDefinition> entry in value)
            {
                emitter.WriteScalar(entry.Key);
                serialiser(entry.Value, typeof(DependencyDefinition));
            }

            emitter.EndMapping();
        }
    }
}
