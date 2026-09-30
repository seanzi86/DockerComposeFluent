using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ResourceLimitsDefinition"/> as the <c>deploy.resources.limits</c> entry.
    /// </summary>
    internal sealed class ResourceLimitsDefinitionYamlConverter : YamlConverter<ResourceLimitsDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ResourceLimitsDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("cpus", value.Cpus);
            emitter.WriteOptionalScalar("memory", value.Memory);
            emitter.WriteOptionalInteger("pids", value.Pids);
            emitter.EndMapping();
        }
    }
}
