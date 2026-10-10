using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="WatchRuleDefinition"/> as one <c>develop.watch</c> rule.
    /// </summary>
    internal sealed class WatchRuleDefinitionYamlConverter : YamlConverter<WatchRuleDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, WatchRuleDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalScalar("action", value.Action == null ? null : EnumNames.Of(value.Action.Value));
            emitter.WriteOptionalValue("exec", value.Exec, serialiser);
            emitter.WriteOptionalStringSequence("ignore", value.Ignore);
            emitter.WriteOptionalStringSequence("include", value.Include);
            emitter.WriteOptionalBoolean("initial_sync", value.InitialSync);
            emitter.WriteOptionalScalar("path", value.Path);
            emitter.WriteOptionalScalar("target", value.Target);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
