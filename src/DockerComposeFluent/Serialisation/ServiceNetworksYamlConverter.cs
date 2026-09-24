using System.Collections.Generic;
using System.Linq;
using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes the networks of a service as a list of names when no network has settings, or as a mapping
    /// of names to settings otherwise, since Compose does not allow the two forms to be mixed.
    /// </summary>
    internal sealed class ServiceNetworksYamlConverter : YamlConverter<IReadOnlyDictionary<string, NetworkAttachment>>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, IReadOnlyDictionary<string, NetworkAttachment> value, ObjectSerializer serialiser)
        {
            if (value.Values.All(attachment => attachment.IsEmpty))
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
            foreach (KeyValuePair<string, NetworkAttachment> entry in value)
            {
                emitter.WriteScalar(entry.Key);

                if (entry.Value.IsEmpty)
                {
                    emitter.StartMapping();
                    emitter.EndMapping();
                }
                else
                {
                    serialiser(entry.Value, typeof(NetworkAttachment));
                }
            }

            emitter.EndMapping();
        }
    }
}
