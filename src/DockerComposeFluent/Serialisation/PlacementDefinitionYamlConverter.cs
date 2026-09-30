using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="PlacementDefinition"/> as the <c>deploy.placement</c> entry.
    /// </summary>
    internal sealed class PlacementDefinitionYamlConverter : YamlConverter<PlacementDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, PlacementDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalStringSequence("constraints", value.Constraints);
            emitter.WriteOptionalInteger("max_replicas_per_node", value.MaxReplicasPerNode);

            if (value.Preferences.Count > 0)
            {
                emitter.WriteKey("preferences");
                emitter.StartSequence(SequenceStyle.Block);

                foreach (string preference in value.Preferences)
                {
                    emitter.StartMapping();
                    emitter.WriteScalarEntry("spread", preference);
                    emitter.EndMapping();
                }

                emitter.EndSequence();
            }

            emitter.EndMapping();
        }
    }
}
