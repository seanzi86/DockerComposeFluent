using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes an <see cref="UpdateConfigDefinition"/> as the <c>deploy.update_config</c> entry.
    /// </summary>
    internal sealed class UpdateConfigDefinitionYamlConverter : YamlConverter<UpdateConfigDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, UpdateConfigDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("delay", value.Delay);
            emitter.WriteOptionalScalar("failure_action", EnumNames.Of(value.FailureAction));
            emitter.WriteOptionalNumber("max_failure_ratio", value.MaxFailureRatio);
            emitter.WriteOptionalScalar("monitor", value.Monitor);
            emitter.WriteOptionalScalar("order", EnumNames.Of(value.Order));
            emitter.WriteOptionalInteger("parallelism", value.Parallelism);
            emitter.EndMapping();
        }
    }
}
