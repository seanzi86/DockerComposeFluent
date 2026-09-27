using System.Collections.Generic;
using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes <see cref="Labels"/> as a mapping, or as a list of <c>KEY=value</c> strings when the list form
    /// was asked for.
    /// </summary>
    internal sealed class LabelsYamlConverter : YamlConverter<Labels>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, Labels value, ObjectSerializer serialiser)
        {
            if (value.UsesListSyntax)
            {
                emitter.StartSequence(SequenceStyle.Block);
                foreach (KeyValuePair<string, string> label in value.Values)
                {
                    emitter.WriteScalar(label.Key + "=" + label.Value);
                }

                emitter.EndSequence();
                return;
            }

            emitter.StartMapping();
            foreach (KeyValuePair<string, string> label in value.Values)
            {
                emitter.WriteScalarEntry(label.Key, label.Value);
            }

            emitter.EndMapping();
        }
    }
}
