using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="GpusDefinition"/> as the string <c>all</c> or a list of GPU requests.
    /// </summary>
    internal sealed class GpusDefinitionYamlConverter : YamlConverter<GpusDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, GpusDefinition value, ObjectSerializer serialiser)
        {
            if (value.All)
            {
                emitter.WriteScalar("all");
                return;
            }

            emitter.StartSequence(SequenceStyle.Block);
            foreach (GpuDefinition device in value.Devices)
            {
                serialiser(device, typeof(GpuDefinition));
            }

            emitter.EndSequence();
        }
    }
}
