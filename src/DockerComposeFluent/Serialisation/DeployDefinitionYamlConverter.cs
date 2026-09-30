using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DeployDefinition"/> as a service's <c>deploy</c> entry.
    /// </summary>
    internal sealed class DeployDefinitionYamlConverter : YamlConverter<DeployDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DeployDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("endpoint_mode", EnumNames.Of(value.EndpointMode));

            if (value.Labels.Values.Count > 0)
            {
                emitter.WriteOptionalValue("labels", value.Labels, serialiser);
            }

            emitter.WriteOptionalScalar("mode", EnumNames.Of(value.Mode));
            emitter.WriteOptionalValue("placement", value.Placement, serialiser);
            emitter.WriteOptionalInteger("replicas", value.Replicas);
            emitter.WriteOptionalValue("resources", value.Resources, serialiser);
            emitter.WriteOptionalValue("restart_policy", value.RestartPolicy, serialiser);
            emitter.WriteOptionalValue("rollback_config", value.RollbackConfig, serialiser);
            emitter.WriteOptionalValue("update_config", value.UpdateConfig, serialiser);
            emitter.EndMapping();
        }
    }
}
