using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="PreStartDefinition"/> as one <c>pre_start</c> step.
    /// </summary>
    internal sealed class PreStartDefinitionYamlConverter : YamlConverter<PreStartDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, PreStartDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalValue("command", value.Command, serialiser);
            emitter.WriteOptionalStringMap("environment", value.Environment);
            emitter.WriteOptionalScalar("image", value.Image);
            emitter.WriteOptionalBoolean("per_replica", value.PerReplica);
            emitter.WriteOptionalBoolean("privileged", value.Privileged);
            emitter.WriteOptionalScalar("user", value.User);
            emitter.WriteOptionalScalar("working_dir", value.WorkingDir);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
