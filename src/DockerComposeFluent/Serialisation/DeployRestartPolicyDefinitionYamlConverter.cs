using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="DeployRestartPolicyDefinition"/> as the <c>deploy.restart_policy</c> entry.
    /// </summary>
    internal sealed class DeployRestartPolicyDefinitionYamlConverter : YamlConverter<DeployRestartPolicyDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, DeployRestartPolicyDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("condition", EnumNames.Of(value.Condition));
            emitter.WriteOptionalScalar("delay", value.Delay);
            emitter.WriteOptionalInteger("max_attempts", value.MaxAttempts);
            emitter.WriteOptionalScalar("window", value.Window);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
