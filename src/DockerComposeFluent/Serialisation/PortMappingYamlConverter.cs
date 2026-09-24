using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="PortMapping"/> as a string (short syntax) or a mapping (long syntax).
    /// </summary>
    internal sealed class PortMappingYamlConverter : YamlConverter<PortMapping>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, PortMapping value, ObjectSerializer serialiser)
        {
            if (value.ShortSyntax != null)
            {
                emitter.WriteScalar(value.ShortSyntax);
                return;
            }

            serialiser(value.Definition, typeof(PortDefinition));
        }
    }
}
