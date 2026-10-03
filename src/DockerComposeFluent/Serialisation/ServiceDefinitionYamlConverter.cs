using DockerComposeFluent.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// Writes a <see cref="ServiceDefinition"/> as a service entry.
    /// </summary>
    internal sealed class ServiceDefinitionYamlConverter : YamlConverter<ServiceDefinition>
    {
        /// <inheritdoc />
        protected override void Write(IEmitter emitter, ServiceDefinition value, ObjectSerializer serialiser)
        {
            emitter.StartMapping();
            emitter.WriteOptionalBoolean("attach", value.Attach);
            emitter.WriteOptionalValue("command", value.Command, serialiser);
            emitter.WriteOptionalScalar("container_name", value.ContainerName);
            emitter.WriteOptionalSequence("configs", value.Configs, serialiser);

            if (value.DependsOn.Count > 0)
            {
                emitter.WriteOptionalValue("depends_on", value.DependsOn, serialiser);
            }

            emitter.WriteOptionalValue("deploy", value.Deploy, serialiser);
            emitter.WriteOptionalScalar("domainname", value.DomainName);
            emitter.WriteOptionalValue("entrypoint", value.Entrypoint, serialiser);

            emitter.WriteOptionalSequence("env_file", value.EnvFiles, serialiser);

            if (value.Environment.Variables.Count > 0)
            {
                emitter.WriteOptionalValue("environment", value.Environment, serialiser);
            }

            emitter.WriteOptionalValue("extends", value.Extends, serialiser);
            emitter.WriteOptionalValue("healthcheck", value.Healthcheck, serialiser);
            emitter.WriteOptionalScalar("hostname", value.Hostname);
            emitter.WriteOptionalScalar("image", value.Image);
            emitter.WriteOptionalBoolean("init", value.Init);
            emitter.WriteOptionalScalar("isolation", value.Isolation);
            emitter.WriteOptionalStringSequence("label_file", value.LabelFiles);

            if (value.Labels.Values.Count > 0)
            {
                emitter.WriteOptionalValue("labels", value.Labels, serialiser);
            }


            emitter.WriteOptionalScalar("mac_address", value.MacAddress);

            if (value.Networks.Count > 0)
            {
                emitter.WriteOptionalValue("networks", value.Networks, serialiser);
            }

            emitter.WriteOptionalScalar("platform", value.Platform);
            emitter.WriteOptionalSequence("ports", value.Ports, serialiser);
            emitter.WriteOptionalBoolean("privileged", value.Privileged);
            emitter.WriteOptionalScalar("pull_policy", value.PullPolicy);
            emitter.WriteOptionalScalar("pull_refresh_after", value.PullRefreshAfter);
            emitter.WriteOptionalValue("logging", value.Logging, serialiser);
            emitter.WriteOptionalStringSequence("profiles", value.Profiles);
            emitter.WriteOptionalSequence("secrets", value.Secrets, serialiser);
            emitter.WriteOptionalValue("restart", value.Restart, serialiser);
            emitter.WriteOptionalBoolean("read_only", value.ReadOnly);
            emitter.WriteOptionalScalar("runtime", value.Runtime);
            emitter.WriteOptionalInteger("scale", value.Scale);
            emitter.WriteOptionalBoolean("stdin_open", value.StdinOpen);
            emitter.WriteOptionalScalar("stop_grace_period", value.StopGracePeriod);
            emitter.WriteOptionalScalar("stop_signal", value.StopSignal);
            emitter.WriteOptionalBoolean("tty", value.Tty);
            emitter.WriteOptionalBoolean("use_api_socket", value.UseApiSocket);
            emitter.WriteOptionalScalar("user", value.User);
            emitter.WriteOptionalScalar("working_dir", value.WorkingDir);
            emitter.WriteOptionalSequence("volumes", value.Volumes, serialiser);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
