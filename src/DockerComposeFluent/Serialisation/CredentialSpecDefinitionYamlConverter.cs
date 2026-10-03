using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="CredentialSpecDefinition"/> as a service's <c>credential_spec</c> entry.
    /// </summary>
    internal sealed class CredentialSpecDefinitionYamlConverter : YamlConverter<CredentialSpecDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, CredentialSpecDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("config", value.Config);
            emitter.WriteOptionalScalar("file", value.File);
            emitter.WriteOptionalScalar("registry", value.Registry);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
