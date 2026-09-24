using System.Collections.Generic;
using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes <see cref="EnvironmentVariables"/> as a mapping, or as a list of <c>KEY=value</c> strings
    /// when the list form was asked for.
    /// </summary>
    internal sealed class EnvironmentVariablesYamlConverter : YamlConverter<EnvironmentVariables>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, EnvironmentVariables value, ObjectSerializer serialiser)
        {
            if (value.UsesListSyntax)
            {
                emitter.StartSequence(SequenceStyle.Block);
                foreach (KeyValuePair<string, string?> variable in value.Variables)
                {
                    emitter.WriteScalar(variable.Value == null ? variable.Key : variable.Key + "=" + variable.Value);
                }

                emitter.EndSequence();
                return;
            }

            emitter.StartMapping();
            foreach (KeyValuePair<string, string?> variable in value.Variables)
            {
                emitter.WriteScalar(variable.Key);

                if (variable.Value == null)
                {
                    emitter.WriteNull();
                }
                else
                {
                    emitter.WriteScalar(variable.Value);
                }
            }

            emitter.EndMapping();
        }
    }
}
