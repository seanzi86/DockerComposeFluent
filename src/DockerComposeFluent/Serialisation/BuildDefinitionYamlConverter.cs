using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="BuildDefinition"/> as a plain context string when only the context is set, or as a
    /// mapping otherwise.
    /// </summary>
    internal sealed class BuildDefinitionYamlConverter : YamlConverter<BuildDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, BuildDefinition value, ObjectSerializer serialiser)
        {
            if (value.IsContextOnly)
            {
                emitter.WriteScalar(value.Context!);
                return;
            }

            emitter.StartMapping();
            emitter.WriteOptionalStringMap("additional_contexts", value.AdditionalContexts);
            emitter.WriteOptionalStringMap("args", value.Args);
            emitter.WriteOptionalStringSequence("cache_from", value.CacheFrom);
            emitter.WriteOptionalStringSequence("cache_to", value.CacheTo);
            emitter.WriteOptionalScalar("context", value.Context);
            emitter.WriteOptionalScalar("dockerfile", value.Dockerfile);
            emitter.WriteOptionalScalar("dockerfile_inline", value.DockerfileInline);
            emitter.WriteOptionalStringSequence("entitlements", value.Entitlements);
            emitter.WriteOptionalExtraHosts("extra_hosts", value.ExtraHosts);
            emitter.WriteOptionalScalar("isolation", value.Isolation);
            if (value.Labels.Values.Count > 0)
            {
                emitter.WriteOptionalValue("labels", value.Labels, serialiser);
            }

            emitter.WriteOptionalScalar("network", value.Network);
            emitter.WriteOptionalBoolean("no_cache", value.NoCache);
            emitter.WriteOptionalStringSequence("no_cache_filter", value.NoCacheFilter);
            emitter.WriteOptionalStringSequence("platforms", value.Platforms);
            emitter.WriteOptionalBoolean("privileged", value.Privileged);
            emitter.WriteOptionalBooleanOrString("provenance", value.Provenance);
            emitter.WriteOptionalBoolean("pull", value.Pull);
            emitter.WriteOptionalBooleanOrString("sbom", value.Sbom);
            emitter.WriteOptionalSequence("secrets", value.Secrets, serialiser);
            emitter.WriteOptionalScalar("shm_size", value.ShmSize);
            emitter.WriteOptionalStringSequence("ssh", value.Ssh);
            emitter.WriteOptionalStringSequence("tags", value.Tags);
            emitter.WriteOptionalScalar("target", value.Target);
            emitter.WriteMap("ulimits", value.Ulimits, serialiser);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
