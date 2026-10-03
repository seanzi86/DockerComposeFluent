using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ServiceHookDefinition"/> as one lifecycle hook.
    /// </summary>
    internal sealed class ServiceHookDefinitionYamlConverter : YamlConverter<ServiceHookDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ServiceHookDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalValue("command", value.Command, serialiser);
            emitter.WriteOptionalStringMap("environment", value.Environment);
            emitter.WriteOptionalBoolean("privileged", value.Privileged);
            emitter.WriteOptionalScalar("user", value.User);
            emitter.WriteOptionalScalar("working_dir", value.WorkingDir);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
