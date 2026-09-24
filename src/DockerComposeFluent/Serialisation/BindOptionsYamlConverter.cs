using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes <see cref="BindOptions"/> as the <c>bind</c> mapping of a mount.
    /// </summary>
    internal sealed class BindOptionsYamlConverter : YamlConverter<BindOptions>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, BindOptions value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalBoolean("create_host_path", value.CreateHostPath);
            emitter.WriteOptionalScalar("propagation", EnumNames.Of(value.Propagation));
            emitter.WriteOptionalScalar("selinux", EnumNames.Of(value.Selinux));
            emitter.EndMapping();
        }
    }
}
