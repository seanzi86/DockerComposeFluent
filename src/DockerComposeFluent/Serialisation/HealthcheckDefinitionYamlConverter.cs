using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="HealthcheckDefinition"/> as a service's <c>healthcheck</c> entry.
    /// </summary>
    internal sealed class HealthcheckDefinitionYamlConverter : YamlConverter<HealthcheckDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, HealthcheckDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalBoolean("disable", value.Disable);
            emitter.WriteOptionalScalar("interval", value.Interval);
            emitter.WriteOptionalInteger("retries", value.Retries);
            emitter.WriteOptionalScalar("start_interval", value.StartInterval);
            emitter.WriteOptionalScalar("start_period", value.StartPeriod);
            emitter.WriteOptionalValue("test", value.Test, serialiser);
            emitter.WriteOptionalScalar("timeout", value.Timeout);
            emitter.EndMapping();
        }
    }
}
