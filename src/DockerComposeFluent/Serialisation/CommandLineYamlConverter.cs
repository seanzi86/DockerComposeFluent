using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="CommandLine"/> as a string (shell form) or a list (exec form).
    /// </summary>
    internal sealed class CommandLineYamlConverter : YamlConverter<CommandLine>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, CommandLine value, ObjectSerializer serialiser)
        {
            if (value.Shell != null)
            {
                emitter.WriteScalar(value.Shell);
                return;
            }

            emitter.StartSequence(SequenceStyle.Flow);
            foreach (string argument in value.Arguments!)
            {
                emitter.WriteScalar(argument);
            }

            emitter.EndSequence();
        }
    }
}
