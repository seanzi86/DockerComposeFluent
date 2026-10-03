using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="UlimitDefinition"/> as a plain number when it is a single value, or a mapping otherwise.
    /// </summary>
    internal sealed class UlimitDefinitionYamlConverter : YamlConverter<UlimitDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, UlimitDefinition value, ObjectSerializer serialiser)
        {
            if (value.IsSingle)
            {
                emitter.Emit(new YamlDotNet.Core.Events.Scalar(value.Single!.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalInteger("hard", value.Hard);
            emitter.WriteOptionalInteger("soft", value.Soft);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
