using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="RollbackConfigDefinition"/> as the <c>deploy.rollback_config</c> entry.
    /// </summary>
    internal sealed class RollbackConfigDefinitionYamlConverter : YamlConverter<RollbackConfigDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, RollbackConfigDefinition value, ObjectSerializer serialiser)
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
