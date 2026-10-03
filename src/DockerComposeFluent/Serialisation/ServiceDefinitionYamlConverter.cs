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
            emitter.WriteOptionalStringSequence("cap_add", value.CapAdd);
            emitter.WriteOptionalStringSequence("cap_drop", value.CapDrop);
            emitter.WriteOptionalScalar("cgroup", EnumNames.Of(value.Cgroup));
            emitter.WriteOptionalScalar("cgroup_parent", value.CgroupParent);
            emitter.WriteOptionalInteger("cpu_count", value.CpuCount);
            emitter.WriteOptionalInteger("cpu_percent", value.CpuPercent);
            emitter.WriteOptionalLong("cpu_period", value.CpuPeriod);
            emitter.WriteOptionalLong("cpu_quota", value.CpuQuota);
            emitter.WriteOptionalLong("cpu_rt_period", value.CpuRtPeriod);
            emitter.WriteOptionalLong("cpu_rt_runtime", value.CpuRtRuntime);
            emitter.WriteOptionalLong("cpu_shares", value.CpuShares);
            emitter.WriteOptionalScalar("cpus", value.Cpus);
            emitter.WriteOptionalScalar("cpuset", value.Cpuset);
            emitter.WriteOptionalStringSequence("device_cgroup_rules", value.DeviceCgroupRules);
            emitter.WriteOptionalStringSequence("dns", value.Dns);
            emitter.WriteOptionalStringSequence("dns_opt", value.DnsOpt);
            emitter.WriteOptionalStringSequence("dns_search", value.DnsSearch);
            emitter.WriteOptionalStringSequence("expose", value.Expose);
            emitter.WriteOptionalStringSequence("external_links", value.ExternalLinks);
            emitter.WriteOptionalStringSequence("group_add", value.GroupAdd);
            emitter.WriteOptionalScalar("ipc", value.Ipc);
            emitter.WriteOptionalStringSequence("links", value.Links);
            emitter.WriteOptionalScalar("mem_limit", value.MemLimit);
            emitter.WriteOptionalScalar("mem_reservation", value.MemReservation);
            emitter.WriteOptionalInteger("mem_swappiness", value.MemSwappiness);
            emitter.WriteOptionalScalar("memswap_limit", value.MemswapLimit);
            emitter.WriteOptionalScalar("network_mode", value.NetworkMode);
            emitter.WriteOptionalBoolean("oom_kill_disable", value.OomKillDisable);
            emitter.WriteOptionalInteger("oom_score_adj", value.OomScoreAdj);
            emitter.WriteOptionalScalar("pid", value.Pid);
            emitter.WriteOptionalInteger("pids_limit", value.PidsLimit);
            emitter.WriteOptionalStringSequence("security_opt", value.SecurityOpt);
            emitter.WriteOptionalScalar("shm_size", value.ShmSize);
            emitter.WriteOptionalStringSequence("tmpfs", value.Tmpfs);
            emitter.WriteOptionalScalar("userns_mode", value.UsernsMode);
            emitter.WriteOptionalScalar("uts", value.Uts);
            emitter.WriteOptionalStringSequence("volumes_from", value.VolumesFrom);
            emitter.WriteOptionalValue("blkio_config", value.BlkioConfig, serialiser);
            emitter.WriteOptionalValue("credential_spec", value.CredentialSpec, serialiser);
            emitter.WriteOptionalSequence("devices", value.Devices, serialiser);
            emitter.WriteOptionalExtraHosts("extra_hosts", value.ExtraHosts);
            emitter.WriteOptionalValue("gpus", value.Gpus, serialiser);
            emitter.WriteOptionalStringMap("storage_opt", value.StorageOpt);
            emitter.WriteOptionalStringMap("sysctls", value.Sysctls);
            emitter.WriteMap("ulimits", value.Ulimits, serialiser);
            emitter.WriteOptionalSequence("volumes", value.Volumes, serialiser);
            emitter.WriteExtensions(value.Extensions, serialiser);
            emitter.EndMapping();
        }
    }
}
