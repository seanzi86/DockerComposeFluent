using System;
using System.Collections.Generic;
using System.Globalization;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="ServiceDefinition"/>.
    /// </summary>
    public sealed class ServiceBuilder
    {
        private ServiceDefinition _definition = new ServiceDefinition();

        /// <summary>
        /// Sets a label, keeping the mapping form. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels"/>
        /// </summary>
        /// <param name="key">The label key, which must not start with the reserved <c>com.docker.compose</c>
        /// prefix.</param>
        /// <param name="value">The label value, which may be empty.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLabel(string key, string value)
        {
            _definition = _definition with { Labels = LabelsMutator.Set(_definition.Labels, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Sets several labels from key/value pairs, keeping the mapping form.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels"/>
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLabels(IEnumerable<KeyValuePair<string, string>> labels)
        {
            Guard.NotNull(labels, nameof(labels));

            foreach (KeyValuePair<string, string> label in labels)
            {
                WithLabel(label.Key, label.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets several labels from <c>KEY=value</c> strings, or a bare <c>KEY</c> for an empty value. Makes the
        /// YAML use the list form (<c>- KEY=value</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#labels"/>
        /// </summary>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLabels(IEnumerable<string> entries)
        {
            _definition = _definition with { Labels = LabelsMutator.SetFromEntries(_definition.Labels, entries, nameof(entries)) };
            return this;
        }

        /// <summary>
        /// Adds a file to load labels from. Files are read in the order added; a later file overrides values
        /// from an earlier one, and a label set directly with <see cref="WithLabel(string, string)"/> overrides
        /// either.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#label_file"/>
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        /// <param name="path">The path, resolved relative to the Compose file's folder.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLabelFile(string path)
        {
            Guard.NotNullOrWhiteSpace(path, nameof(path));
            _definition = _definition with { LabelFiles = Collections.Append(_definition.LabelFiles, path) };
            return this;
        }

        /// <summary>
        /// Adds several files to load labels from.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#label_file"/>
        /// </summary>
        /// <remarks>Requires Compose 2.32.0 or later.</remarks>
        /// <param name="paths">The paths, resolved relative to the Compose file's folder.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLabelFiles(IEnumerable<string> paths)
        {
            Guard.NotNull(paths, nameof(paths));

            foreach (string path in paths)
            {
                WithLabelFile(path);
            }

            return this;
        }

        /// <summary>
        /// Sets the healthcheck to a shell-form command (Compose runs it with <c>CMD-SHELL</c>). Use
        /// <see cref="WithHealthcheck(Action{HealthcheckBuilder})"/> to set the interval, timeout and so on.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="test">The command, for example <c>curl -f http://localhost || exit 1</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithHealthcheck(string test)
        {
            return WithHealthcheck(new HealthcheckDefinition { Test = CommandLine.FromShell(test) });
        }

        /// <summary>
        /// Sets the healthcheck from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="healthcheck">The healthcheck definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithHealthcheck(HealthcheckDefinition healthcheck)
        {
            Guard.NotNull(healthcheck, nameof(healthcheck));
            _definition = _definition with { Healthcheck = healthcheck };
            return this;
        }

        /// <summary>
        /// Sets the healthcheck, configured through a <see cref="HealthcheckBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <param name="configure">Configures the healthcheck.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithHealthcheck(Action<HealthcheckBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            HealthcheckBuilder builder = new HealthcheckBuilder();
            configure(builder);
            return WithHealthcheck(builder.Build());
        }

        /// <summary>
        /// Disables the healthcheck set by the image (written as <c>healthcheck: disable: true</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#healthcheck"/>
        /// </summary>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithoutHealthcheck()
        {
            return WithHealthcheck(new HealthcheckDefinition { Disable = true });
        }

        /// <summary>
        /// Sets the image to start the container from.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#image"/>
        /// </summary>
        /// <param name="image">The image name, optionally including a tag or digest.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithImage(string image)
        {
            Guard.NotNullOrWhiteSpace(image, nameof(image));
            _definition = _definition with { Image = image };
            return this;
        }

        /// <summary>
        /// Sets the command as a single shell-form string, written to YAML as a string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#command"/>
        /// </summary>
        /// <param name="command">The command, for example <c>nginx -g "daemon off;"</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCommand(string command)
        {
            _definition = _definition with { Command = CommandLine.FromShell(command) };
            return this;
        }

        /// <summary>
        /// Sets the command as an exec-form list of arguments, written to YAML as a list.
        /// An empty list clears the command defined by the image.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#command"/>
        /// </summary>
        /// <param name="arguments">The arguments, for example <c>nginx</c>, <c>-g</c>, <c>daemon off;</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCommand(IEnumerable<string> arguments)
        {
            _definition = _definition with { Command = CommandLine.FromArguments(arguments) };
            return this;
        }

        /// <summary>
        /// Sets a custom container name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#container_name"/>
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithContainerName(string containerName)
        {
            Guard.NotNullOrWhiteSpace(containerName, nameof(containerName));
            _definition = _definition with { ContainerName = containerName };
            return this;
        }

        /// <summary>
        /// Makes the service depend on another service with no settings. When no dependency has settings, they
        /// are written to YAML as a plain list of names. Depending on the same service again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#depends_on"/>
        /// </summary>
        /// <param name="service">The name of another service in the file.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDependsOn(string service)
        {
            return AddDependency(service, new DependencyDefinition());
        }

        /// <summary>
        /// Makes the service depend on several other services, each with no settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#depends_on"/>
        /// </summary>
        /// <param name="services">The names of other services in the file.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDependsOn(IEnumerable<string> services)
        {
            Guard.NotNull(services, nameof(services));

            foreach (string service in services)
            {
                WithDependsOn(service);
            }

            return this;
        }

        /// <summary>
        /// Makes the service depend on another service until a condition is met. Any dependency with settings
        /// makes the dependencies be written to YAML as a mapping, including when the condition is
        /// <see cref="DependencyCondition.ServiceStarted"/>, since it was asked for explicitly.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <param name="service">The name of another service in the file.</param>
        /// <param name="condition">When the dependency is considered satisfied.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDependsOn(string service, DependencyCondition condition)
        {
            return AddDependency(service, new DependencyDefinition { Condition = condition });
        }

        /// <summary>
        /// Makes the service depend on another service with existing settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <param name="service">The name of another service in the file.</param>
        /// <param name="dependency">The settings for this dependency.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDependsOn(string service, DependencyDefinition dependency)
        {
            Guard.NotNull(dependency, nameof(dependency));

            return AddDependency(service, dependency);
        }

        /// <summary>
        /// Makes the service depend on another service, with settings configured through a
        /// <see cref="DependencyBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#long-syntax"/>
        /// </summary>
        /// <param name="service">The name of another service in the file.</param>
        /// <param name="configure">Configures the dependency.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDependsOn(string service, Action<DependencyBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            DependencyBuilder builder = new DependencyBuilder();
            configure(builder);
            return AddDependency(service, builder.Build());
        }

        /// <summary>
        /// Sets deployment metadata for the service, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md"/>
        /// </summary>
        /// <param name="deploy">The deploy definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDeploy(DeployDefinition deploy)
        {
            Guard.NotNull(deploy, nameof(deploy));
            _definition = _definition with { Deploy = deploy };
            return this;
        }

        /// <summary>
        /// Sets deployment metadata for the service, configured through a <see cref="DeployBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/deploy.md"/>
        /// </summary>
        /// <param name="configure">Configures the deployment metadata.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDeploy(Action<DeployBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            DeployBuilder builder = new DeployBuilder();
            configure(builder);
            return WithDeploy(builder.Build());
        }

        /// <summary>
        /// Sets the entrypoint as a single shell-form string, written to YAML as a string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint"/>
        /// </summary>
        /// <param name="entrypoint">The entrypoint, for example <c>nginx -g "daemon off;"</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEntrypoint(string entrypoint)
        {
            _definition = _definition with { Entrypoint = CommandLine.FromShell(entrypoint) };
            return this;
        }

        /// <summary>
        /// Sets the entrypoint as an exec-form list of arguments, written to YAML as a list.
        /// An empty list clears the entrypoint defined by the image.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#entrypoint"/>
        /// </summary>
        /// <param name="arguments">The arguments, for example <c>nginx</c>, <c>-g</c>, <c>daemon off;</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEntrypoint(IEnumerable<string> arguments)
        {
            _definition = _definition with { Entrypoint = CommandLine.FromArguments(arguments) };
            return this;
        }

        /// <summary>
        /// Sets the other service this service extends, referencing a service in the same file, written to
        /// YAML as a plain string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
        /// </summary>
        /// <param name="service">The name of the service being referenced as a base.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExtends(string service)
        {
            Guard.NotNullOrWhiteSpace(service, nameof(service));
            return WithExtends(new ExtendsDefinition { Service = service });
        }

        /// <summary>
        /// Sets the other service this service extends, referencing a service in another Compose file.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
        /// </summary>
        /// <param name="service">The name of the service being referenced as a base.</param>
        /// <param name="file">The Compose file it is defined in. Relative paths are resolved against the
        /// main Compose file's location.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExtends(string service, string file)
        {
            Guard.NotNullOrWhiteSpace(service, nameof(service));
            Guard.NotNullOrWhiteSpace(file, nameof(file));
            return WithExtends(new ExtendsDefinition { Service = service, File = file });
        }

        /// <summary>
        /// Sets the other service this service extends, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
        /// </summary>
        /// <param name="extends">The extends definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExtends(ExtendsDefinition extends)
        {
            Guard.NotNull(extends, nameof(extends));
            _definition = _definition with { Extends = extends };
            return this;
        }

        /// <summary>
        /// Sets the other service this service extends, configured through an <see cref="ExtendsBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extends"/>
        /// </summary>
        /// <param name="configure">Configures the extends definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExtends(Action<ExtendsBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ExtendsBuilder builder = new ExtendsBuilder();
            configure(builder);
            return WithExtends(builder.Build());
        }

        /// <summary>
        /// Adds a port mapping in short syntax, written to YAML as a string. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="port">The mapping, for example <c>8080:80</c> or <c>127.0.0.1:8080:80/udp</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(string port)
        {
            return AddPort(PortMapping.FromShortSyntax(port));
        }

        /// <summary>
        /// Adds a container port with no published port, so the runtime assigns a host port, written to YAML
        /// in long syntax. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="target">The container port, from 1 to 65535.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(int target)
        {
            return AddPort(PortMapping.FromDefinition(new PortDefinition(target)));
        }

        /// <summary>
        /// Adds a port mapping from a published and a target port, written to YAML in long syntax.
        /// Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="published">The publicly exposed port, from 1 to 65535.</param>
        /// <param name="target">The container port, from 1 to 65535.</param>
        /// <param name="protocol">The protocol, or <c>null</c> to leave it unset.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(int published, int target, PortProtocol? protocol = null)
        {
            Guard.ValidPort(published, nameof(published));

            PortDefinition definition = new PortDefinition(target)
            {
                Protocol = protocol,
                Published = published.ToString(CultureInfo.InvariantCulture)
            };

            return AddPort(PortMapping.FromDefinition(definition));
        }

        /// <summary>
        /// Adds a port mapping from an existing long-syntax definition. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="port">The port definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(PortDefinition port)
        {
            return AddPort(PortMapping.FromDefinition(port));
        }

        /// <summary>
        /// Adds a port mapping configured through a <see cref="PortBuilder"/>. Each call adds another port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="configure">Configures the port.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPort(Action<PortBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            PortBuilder builder = new PortBuilder();
            configure(builder);
            return AddPort(PortMapping.FromDefinition(builder.Build()));
        }

        /// <summary>
        /// Adds several port mappings in short syntax.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="ports">The mappings, for example <c>8080:80</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPorts(IEnumerable<string> ports)
        {
            Guard.NotNull(ports, nameof(ports));

            foreach (string port in ports)
            {
                WithPort(port);
            }

            return this;
        }

        /// <summary>
        /// Adds several port mappings from existing long-syntax definitions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ports"/>
        /// </summary>
        /// <param name="ports">The port definitions.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPorts(IEnumerable<PortDefinition> ports)
        {
            Guard.NotNull(ports, nameof(ports));

            foreach (PortDefinition port in ports)
            {
                WithPort(port);
            }

            return this;
        }

        /// <summary>
        /// Sets when the container is restarted, from a policy that has no retry limit. Use
        /// <see cref="WithRestartOnFailure(int)"/> to give up after a number of retries.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="policy">The restart policy.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithRestart(RestartPolicy policy)
        {
            return WithRestart(RestartDefinition.FromPolicy(policy));
        }

        /// <summary>
        /// Sets when the container is restarted, from its compose-spec text.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="restart">One of <c>no</c>, <c>always</c>, <c>on-failure</c>, <c>on-failure:5</c> or
        /// <c>unless-stopped</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithRestart(string restart)
        {
            return WithRestart(RestartDefinition.Parse(restart));
        }

        /// <summary>
        /// Sets when the container is restarted, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="restart">The restart definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithRestart(RestartDefinition restart)
        {
            Guard.NotNull(restart, nameof(restart));
            _definition = _definition with { Restart = restart };
            return this;
        }

        /// <summary>
        /// Sets when the container is restarted, configured through a <see cref="RestartBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="configure">Configures the restart definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithRestart(Action<RestartBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            RestartBuilder builder = new RestartBuilder();
            configure(builder);
            return WithRestart(builder.Build());
        }

        /// <summary>
        /// Restarts the container when it exits with a non-zero exit code, giving up after a number of attempts
        /// (written as <c>on-failure:&lt;maxRetries&gt;</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#restart"/>
        /// </summary>
        /// <param name="maxRetries">The maximum number of restart attempts, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithRestartOnFailure(int maxRetries)
        {
            return WithRestart(RestartDefinition.OnFailure(maxRetries));
        }

        /// <summary>
        /// Adds a mount in short syntax, written to YAML as a string. Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mount">The mount, for example <c>db-data:/var/lib/db</c> or <c>./src:/app:ro</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(string mount)
        {
            return AddVolume(MountMapping.FromShortSyntax(mount));
        }

        /// <summary>
        /// Adds a mount from a source and a target, written to YAML in short syntax
        /// (<c>source:target</c> or <c>source:target:ro</c>) so that Compose decides whether the source is a
        /// named volume or a host path. Use <see cref="WithVolume(Action{MountBuilder})"/> to state the type.
        /// Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="source">A volume name or a host path.</param>
        /// <param name="target">The path in the container.</param>
        /// <param name="readOnly"><c>true</c> to mount read-only.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(string source, string target, bool readOnly = false)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            Guard.NotNullOrWhiteSpace(target, nameof(target));

            string mount = readOnly ? source + ":" + target + ":ro" : source + ":" + target;
            return AddVolume(MountMapping.FromShortSyntax(mount));
        }

        /// <summary>
        /// Adds a mount from an existing long-syntax definition. Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mount">The mount definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(MountDefinition mount)
        {
            return AddVolume(MountMapping.FromDefinition(mount));
        }

        /// <summary>
        /// Adds a mount configured through a <see cref="MountBuilder"/>. Each call adds another mount.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="configure">Configures the mount.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolume(Action<MountBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            MountBuilder builder = new MountBuilder();
            configure(builder);
            return AddVolume(MountMapping.FromDefinition(builder.Build()));
        }

        /// <summary>
        /// Adds several mounts in short syntax.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mounts">The mounts, for example <c>db-data:/var/lib/db</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolumes(IEnumerable<string> mounts)
        {
            Guard.NotNull(mounts, nameof(mounts));

            foreach (string mount in mounts)
            {
                WithVolume(mount);
            }

            return this;
        }

        /// <summary>
        /// Adds several mounts from existing long-syntax definitions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes"/>
        /// </summary>
        /// <param name="mounts">The mount definitions.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolumes(IEnumerable<MountDefinition> mounts)
        {
            Guard.NotNull(mounts, nameof(mounts));

            foreach (MountDefinition mount in mounts)
            {
                WithVolume(mount);
            }

            return this;
        }

        /// <summary>
        /// Adds an environment file with no settings, written to YAML as a plain path. Each call adds another
        /// file; files are read in the order added, and a later file overrides values from an earlier one.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
        /// </summary>
        /// <param name="path">The path, resolved relative to the Compose file's folder.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvFile(string path)
        {
            return AddEnvFile(new EnvFileEntry { Path = path });
        }

        /// <summary>
        /// Adds an environment file, stating whether it must exist. Each call adds another file.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#required"/>
        /// </summary>
        /// <remarks>Requires Compose 2.24.0 or later.</remarks>
        /// <param name="path">The path, resolved relative to the Compose file's folder.</param>
        /// <param name="required"><c>false</c> to make the file optional (a missing file is otherwise an
        /// error).</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvFile(string path, bool required)
        {
            return AddEnvFile(new EnvFileEntry { Path = path, Required = required });
        }

        /// <summary>
        /// Adds several environment files, each with no settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
        /// </summary>
        /// <param name="paths">The paths, resolved relative to the Compose file's folder.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvFiles(IEnumerable<string> paths)
        {
            Guard.NotNull(paths, nameof(paths));

            foreach (string path in paths)
            {
                WithEnvFile(path);
            }

            return this;
        }

        /// <summary>
        /// Adds an environment file from an existing entry. Each call adds another file.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvFile(EnvFileEntry entry)
        {
            Guard.NotNull(entry, nameof(entry));
            return AddEnvFile(entry);
        }

        /// <summary>
        /// Adds an environment file, configured through an <see cref="EnvFileEntryBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#env_file"/>
        /// </summary>
        /// <param name="configure">Configures the entry.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvFile(Action<EnvFileEntryBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            EnvFileEntryBuilder builder = new EnvFileEntryBuilder();
            configure(builder);
            return AddEnvFile(builder.Build());
        }

        /// <summary>
        /// Sets the logging driver, with no options. Use
        /// <see cref="WithLogging(Action{LoggingBuilder})"/> to also set options.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging"/>
        /// </summary>
        /// <param name="driver">The driver name, for example <c>json-file</c> or <c>syslog</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLogging(string driver)
        {
            Guard.NotNullOrWhiteSpace(driver, nameof(driver));
            return WithLogging(new LoggingDefinition { Driver = driver });
        }

        /// <summary>
        /// Sets the logging configuration from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging"/>
        /// </summary>
        /// <param name="logging">The logging definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLogging(LoggingDefinition logging)
        {
            Guard.NotNull(logging, nameof(logging));
            _definition = _definition with { Logging = logging };
            return this;
        }

        /// <summary>
        /// Sets the logging configuration, configured through a <see cref="LoggingBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#logging"/>
        /// </summary>
        /// <param name="configure">Configures the logging definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLogging(Action<LoggingBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            LoggingBuilder builder = new LoggingBuilder();
            configure(builder);
            return WithLogging(builder.Build());
        }

        /// <summary>
        /// Adds a profile the service is enabled under. A service with no profiles is always started; one with
        /// profiles only starts when one of them is activated. Each call adds another profile.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#profiles"/>
        /// </summary>
        /// <param name="profile">The profile name: at least two characters, starting with a letter or digit, and
        /// otherwise letters, digits, <c>_</c>, <c>.</c> or <c>-</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithProfile(string profile)
        {
            Guard.ProfileName(profile, nameof(profile));
            _definition = _definition with { Profiles = Collections.Append(_definition.Profiles, profile) };
            return this;
        }

        /// <summary>
        /// Adds several profiles the service is enabled under.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#profiles"/>
        /// </summary>
        /// <param name="profiles">The profile names.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithProfiles(IEnumerable<string> profiles)
        {
            Guard.NotNull(profiles, nameof(profiles));

            foreach (string profile in profiles)
            {
                WithProfile(profile);
            }

            return this;
        }

        /// <summary>
        /// Grants the service access to a config with no settings, written to YAML as a plain source name.
        /// Each call adds another config.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
        /// </summary>
        /// <param name="source">The name of a config defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithConfig(string source)
        {
            return AddConfig(new ConfigReference { Source = source });
        }

        /// <summary>
        /// Grants the service access to several configs, each with no settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
        /// </summary>
        /// <param name="sources">The names of configs defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithConfigs(IEnumerable<string> sources)
        {
            Guard.NotNull(sources, nameof(sources));

            foreach (string source in sources)
            {
                WithConfig(source);
            }

            return this;
        }

        /// <summary>
        /// Grants the service access to a config, mounted under a different file path. Each call adds another
        /// config.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
        /// </summary>
        /// <param name="source">The name of a config defined at the top level.</param>
        /// <param name="target">The path and name of the mounted file.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithConfig(string source, string target)
        {
            return AddConfig(new ConfigReference { Source = source, Target = target });
        }

        /// <summary>
        /// Grants the service access to a config from an existing reference. Each call adds another config.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
        /// </summary>
        /// <param name="config">The config reference.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithConfig(ConfigReference config)
        {
            Guard.NotNull(config, nameof(config));
            return AddConfig(config);
        }

        /// <summary>
        /// Grants the service access to a config, configured through a <see cref="ConfigReferenceBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#configs"/>
        /// </summary>
        /// <param name="configure">Configures the config reference.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithConfig(Action<ConfigReferenceBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ConfigReferenceBuilder builder = new ConfigReferenceBuilder();
            configure(builder);
            return AddConfig(builder.Build());
        }

        /// <summary>
        /// Grants the service access to a secret with no settings, written to YAML as a plain source name.
        /// Each call adds another secret.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
        /// </summary>
        /// <param name="source">The name of a secret defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecret(string source)
        {
            return AddSecret(new SecretReference { Source = source });
        }

        /// <summary>
        /// Grants the service access to several secrets, each with no settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
        /// </summary>
        /// <param name="sources">The names of secrets defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecrets(IEnumerable<string> sources)
        {
            Guard.NotNull(sources, nameof(sources));

            foreach (string source in sources)
            {
                WithSecret(source);
            }

            return this;
        }

        /// <summary>
        /// Grants the service access to a secret, mounted under a different file name. Each call adds another
        /// secret.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
        /// </summary>
        /// <param name="source">The name of a secret defined at the top level.</param>
        /// <param name="target">The name of the mounted file, or an absolute path.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecret(string source, string target)
        {
            return AddSecret(new SecretReference { Source = source, Target = target });
        }

        /// <summary>
        /// Grants the service access to a secret from an existing reference. Each call adds another secret.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
        /// </summary>
        /// <param name="secret">The secret reference.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecret(SecretReference secret)
        {
            Guard.NotNull(secret, nameof(secret));
            return AddSecret(secret);
        }

        /// <summary>
        /// Grants the service access to a secret, configured through a <see cref="SecretReferenceBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#secrets"/>
        /// </summary>
        /// <param name="configure">Configures the secret reference.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecret(Action<SecretReferenceBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            SecretReferenceBuilder builder = new SecretReferenceBuilder();
            configure(builder);
            return AddSecret(builder.Build());
        }

        /// <summary>
        /// Attaches the service to a network with no settings. When no attached network has settings, the
        /// networks are written to YAML as a plain list of names. Attaching the same network again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="name">The name of a network defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetwork(string name)
        {
            return AddNetwork(name, new NetworkAttachment());
        }

        /// <summary>
        /// Attaches the service to a network with existing settings. Any network with settings makes the
        /// networks be written to YAML as a mapping. Attaching the same network again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="name">The name of a network defined at the top level.</param>
        /// <param name="attachment">The settings for this network.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetwork(string name, NetworkAttachment attachment)
        {
            Guard.NotNull(attachment, nameof(attachment));

            return AddNetwork(name, attachment);
        }

        /// <summary>
        /// Attaches the service to a network with settings configured through a
        /// <see cref="NetworkAttachmentBuilder"/>. Attaching the same network again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="name">The name of a network defined at the top level.</param>
        /// <param name="configure">Configures the settings for this network.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetwork(string name, Action<NetworkAttachmentBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            NetworkAttachmentBuilder builder = new NetworkAttachmentBuilder();
            configure(builder);
            return AddNetwork(name, builder.Build());
        }

        /// <summary>
        /// Attaches the service to several networks with no settings.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#networks"/>
        /// </summary>
        /// <param name="names">The names of networks defined at the top level.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetworks(IEnumerable<string> names)
        {
            Guard.NotNull(names, nameof(names));

            foreach (string name in names)
            {
                WithNetwork(name);
            }

            return this;
        }

        /// <summary>
        /// Sets the target platform to run the container on.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#platform"/>
        /// </summary>
        /// <param name="platform">The platform, for example <c>linux/amd64</c> or <c>linux/arm64</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPlatform(string platform)
        {
            Guard.NotNullOrWhiteSpace(platform, nameof(platform));
            _definition = _definition with { Platform = platform };
            return this;
        }

        /// <summary>
        /// Sets an environment variable. Setting the same name again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value, which may be empty.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, string value)
        {
            Guard.NotNull(value, nameof(value));

            return SetEnvironment(key, value, nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to <c>true</c> or <c>false</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, bool value)
        {
            return SetEnvironment(key, value ? "true" : "false", nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to a number.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, int value)
        {
            return SetEnvironment(key, value.ToString(CultureInfo.InvariantCulture), nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to a number.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, long value)
        {
            return SetEnvironment(key, value.ToString(CultureInfo.InvariantCulture), nameof(key));
        }

        /// <summary>
        /// Sets an environment variable to a number, written with a <c>.</c> as the decimal separator.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key, double value)
        {
            return SetEnvironment(key, value.ToString("R", CultureInfo.InvariantCulture), nameof(key));
        }

        /// <summary>
        /// Declares an environment variable with no value, so Compose takes it from the environment of the
        /// machine running it. If it is not set there, the variable is removed from the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="key">The variable name, which must not contain <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(string key)
        {
            return SetEnvironment(key, null, nameof(key));
        }

        /// <summary>
        /// Sets several environment variables from name and value pairs.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="variables">The variables.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(IEnumerable<KeyValuePair<string, string>> variables)
        {
            Guard.NotNull(variables, nameof(variables));

            foreach (KeyValuePair<string, string> variable in variables)
            {
                WithEnvironment(variable.Key, variable.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets several environment variables from <c>KEY=value</c> strings, or a bare <c>KEY</c> for a
        /// variable with no value. Giving a list makes the YAML use the list form (<c>- KEY=value</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#environment"/>
        /// </summary>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithEnvironment(IEnumerable<string> entries)
        {
            Guard.NotNull(entries, nameof(entries));

            foreach (string entry in entries)
            {
                Guard.NotNullOrWhiteSpace(entry, nameof(entries));

                int separator = entry.IndexOf('=');
                if (separator < 0)
                {
                    SetEnvironment(entry, null, nameof(entries));
                }
                else
                {
                    SetEnvironment(entry.Substring(0, separator), entry.Substring(separator + 1), nameof(entries));
                }
            }

            _definition = _definition with { Environment = _definition.Environment with { UsesListSyntax = true } };
            return this;
        }

        /// <summary>
        /// Sets: whether Compose collects the service's logs when attached (<c>docker compose up</c>).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#attach"/>
        /// </summary>
        /// <param name="attach"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithAttach(bool attach)
        {
            _definition = _definition with { Attach = attach };
            return this;
        }

        /// <summary>
        /// Sets: the custom domain name for the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#domainname"/>
        /// </summary>
        /// <param name="domainName">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDomainName(string domainName)
        {
            Guard.NotNullOrWhiteSpace(domainName, nameof(domainName));
            _definition = _definition with { DomainName = domainName };
            return this;
        }

        /// <summary>
        /// Sets: the custom hostname for the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#hostname"/>
        /// </summary>
        /// <param name="hostname">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithHostname(string hostname)
        {
            Guard.NotNullOrWhiteSpace(hostname, nameof(hostname));
            _definition = _definition with { Hostname = hostname };
            return this;
        }

        /// <summary>
        /// Sets: whether to run an init process inside the container that forwards signals and reaps processes.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#init"/>
        /// </summary>
        /// <param name="init"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithInit(bool init)
        {
            _definition = _definition with { Init = init };
            return this;
        }

        /// <summary>
        /// Sets: the container isolation technology; supported values are platform-specific.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#isolation"/>
        /// </summary>
        /// <param name="isolation">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithIsolation(string isolation)
        {
            Guard.NotNullOrWhiteSpace(isolation, nameof(isolation));
            _definition = _definition with { Isolation = isolation };
            return this;
        }

        /// <summary>
        /// Sets: the MAC address of the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#mac_address"/>
        /// </summary>
        /// <param name="macAddress">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithMacAddress(string macAddress)
        {
            Guard.NotNullOrWhiteSpace(macAddress, nameof(macAddress));
            _definition = _definition with { MacAddress = macAddress };
            return this;
        }

        /// <summary>
        /// Sets: whether the container has extended privileges.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#privileged"/>
        /// </summary>
        /// <param name="privileged"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPrivileged(bool privileged)
        {
            _definition = _definition with { Privileged = privileged };
            return this;
        }

        /// <summary>
        /// Sets: when to pull the image: <c>always</c>, <c>never</c>, <c>missing</c>, <c>if_not_present</c>, <c>build</c>, <c>refresh</c>, <c>daily</c>, <c>weekly</c> or <c>every_&lt;duration&gt;</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pull_policy"/>
        /// </summary>
        /// <param name="pullPolicy">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPullPolicy(string pullPolicy)
        {
            Guard.PullPolicy(pullPolicy, nameof(pullPolicy));
            _definition = _definition with { PullPolicy = pullPolicy };
            return this;
        }

        /// <summary>
        /// Sets: how long after which to refresh the image, as a compose-spec duration, used with a <c>refresh</c> pull policy.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pull_refresh_after"/>
        /// </summary>
        /// <param name="pullRefreshAfter">The duration, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPullRefreshAfter(TimeSpan pullRefreshAfter)
        {
            return WithPullRefreshAfter(Durations.Format(pullRefreshAfter, nameof(pullRefreshAfter)));
        }

        /// <summary>
        /// Sets: how long after which to refresh the image, as a compose-spec duration, used with a <c>refresh</c> pull policy.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pull_refresh_after"/>
        /// </summary>
        /// <param name="pullRefreshAfter">The duration, such as <c>30s</c> or <c>1m30s</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPullRefreshAfter(string pullRefreshAfter)
        {
            Guard.Duration(pullRefreshAfter, nameof(pullRefreshAfter));
            _definition = _definition with { PullRefreshAfter = pullRefreshAfter };
            return this;
        }

        /// <summary>
        /// Sets: whether the container's filesystem is mounted read-only.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#read_only"/>
        /// </summary>
        /// <param name="readOnly"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithReadOnly(bool readOnly)
        {
            _definition = _definition with { ReadOnly = readOnly };
            return this;
        }

        /// <summary>
        /// Sets: the runtime to use for the container, for example <c>runc</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#runtime"/>
        /// </summary>
        /// <param name="runtime">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithRuntime(string runtime)
        {
            Guard.NotNullOrWhiteSpace(runtime, nameof(runtime));
            _definition = _definition with { Runtime = runtime };
            return this;
        }

        /// <summary>
        /// Sets: the number of containers to run for the service.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#scale"/>
        /// </summary>
        /// <param name="scale">The number of containers, which must not be negative.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithScale(int scale)
        {
            if (scale < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale), scale, "The scale must not be negative.");
            }

            _definition = _definition with { Scale = scale };
            return this;
        }

        /// <summary>
        /// Sets: whether to keep STDIN open even if not attached.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#stdin_open"/>
        /// </summary>
        /// <param name="stdinOpen"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithStdinOpen(bool stdinOpen)
        {
            _definition = _definition with { StdinOpen = stdinOpen };
            return this;
        }

        /// <summary>
        /// Sets: how long to wait for the container to stop gracefully before sending SIGKILL, as a compose-spec duration.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#stop_grace_period"/>
        /// </summary>
        /// <param name="stopGracePeriod">The duration, which must be positive and a whole number of microseconds.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithStopGracePeriod(TimeSpan stopGracePeriod)
        {
            return WithStopGracePeriod(Durations.Format(stopGracePeriod, nameof(stopGracePeriod)));
        }

        /// <summary>
        /// Sets: how long to wait for the container to stop gracefully before sending SIGKILL, as a compose-spec duration.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#stop_grace_period"/>
        /// </summary>
        /// <param name="stopGracePeriod">The duration, such as <c>30s</c> or <c>1m30s</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithStopGracePeriod(string stopGracePeriod)
        {
            Guard.Duration(stopGracePeriod, nameof(stopGracePeriod));
            _definition = _definition with { StopGracePeriod = stopGracePeriod };
            return this;
        }

        /// <summary>
        /// Sets: the signal used to stop the container, for example <c>SIGTERM</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#stop_signal"/>
        /// </summary>
        /// <param name="stopSignal">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithStopSignal(string stopSignal)
        {
            Guard.NotNullOrWhiteSpace(stopSignal, nameof(stopSignal));
            _definition = _definition with { StopSignal = stopSignal };
            return this;
        }

        /// <summary>
        /// Sets: whether to allocate a pseudo-TTY.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#tty"/>
        /// </summary>
        /// <param name="tty"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithTty(bool tty)
        {
            _definition = _definition with { Tty = tty };
            return this;
        }

        /// <summary>
        /// Sets: whether to bind mount the Docker API socket and its required auth into the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#use_api_socket"/>
        /// </summary>
        /// <param name="useApiSocket"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUseApiSocket(bool useApiSocket)
        {
            _definition = _definition with { UseApiSocket = useApiSocket };
            return this;
        }

        /// <summary>
        /// Sets: the username or UID to run the container process as.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#user"/>
        /// </summary>
        /// <param name="user">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUser(string user)
        {
            Guard.NotNullOrWhiteSpace(user, nameof(user));
            _definition = _definition with { User = user };
            return this;
        }

        /// <summary>
        /// Sets: the working directory the entrypoint or command runs in.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#working_dir"/>
        /// </summary>
        /// <param name="workingDir">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithWorkingDir(string workingDir)
        {
            Guard.NotNullOrWhiteSpace(workingDir, nameof(workingDir));
            _definition = _definition with { WorkingDir = workingDir };
            return this;
        }

        /// <summary>
        /// Sets: the network mode of the container, for example <c>host</c>, <c>none</c> or <c>service:&lt;name&gt;</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#network_mode"/>
        /// </summary>
        /// <param name="networkMode">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithNetworkMode(string networkMode)
        {
            Guard.NotNullOrWhiteSpace(networkMode, nameof(networkMode));
            _definition = _definition with { NetworkMode = networkMode };
            return this;
        }

        /// <summary>
        /// Sets: the IPC isolation mode, for example <c>shareable</c> or <c>service:&lt;name&gt;</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ipc"/>
        /// </summary>
        /// <param name="ipc">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithIpc(string ipc)
        {
            Guard.NotNullOrWhiteSpace(ipc, nameof(ipc));
            _definition = _definition with { Ipc = ipc };
            return this;
        }

        /// <summary>
        /// Sets: the PID namespace mode, for example <c>host</c> or <c>service:&lt;name&gt;</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pid"/>
        /// </summary>
        /// <param name="pid">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPid(string pid)
        {
            Guard.NotNullOrWhiteSpace(pid, nameof(pid));
            _definition = _definition with { Pid = pid };
            return this;
        }

        /// <summary>
        /// Sets: the UTS namespace mode, for example <c>host</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#uts"/>
        /// </summary>
        /// <param name="uts">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUts(string uts)
        {
            Guard.NotNullOrWhiteSpace(uts, nameof(uts));
            _definition = _definition with { Uts = uts };
            return this;
        }

        /// <summary>
        /// Sets: the user namespace mode, for example <c>host</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#userns_mode"/>
        /// </summary>
        /// <param name="usernsMode">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUsernsMode(string usernsMode)
        {
            Guard.NotNullOrWhiteSpace(usernsMode, nameof(usernsMode));
            _definition = _definition with { UsernsMode = usernsMode };
            return this;
        }

        /// <summary>
        /// Sets: the cgroup namespace to join.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cgroup"/>
        /// </summary>
        /// <param name="cgroup">The cgroup namespace.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCgroup(CgroupMode cgroup)
        {
            if (!Enum.IsDefined(typeof(CgroupMode), cgroup))
            {
                throw new ArgumentOutOfRangeException(nameof(cgroup), cgroup, "Unknown CgroupMode value.");
            }

            _definition = _definition with { Cgroup = cgroup };
            return this;
        }

        /// <summary>
        /// Sets: the parent cgroup for the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cgroup_parent"/>
        /// </summary>
        /// <param name="cgroupParent">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCgroupParent(string cgroupParent)
        {
            Guard.NotNullOrWhiteSpace(cgroupParent, nameof(cgroupParent));
            _definition = _definition with { CgroupParent = cgroupParent };
            return this;
        }

        /// <summary>
        /// Sets: the Linux capabilities to add, for example <c>NET_ADMIN</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cap_add"/>
        /// </summary>
        /// <param name="capability">The capability to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCapAdd(string capability)
        {
            Guard.NotNullOrWhiteSpace(capability, nameof(capability));
            _definition = _definition with { CapAdd = Collections.Append(_definition.CapAdd, capability) };
            return this;
        }

        /// <summary>
        /// Sets: the Linux capabilities to add, for example <c>NET_ADMIN</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cap_add"/>
        /// </summary>
        /// <param name="capabilitys">The capabilitys to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCapAdd(IEnumerable<string> capabilitys)
        {
            Guard.NotNull(capabilitys, nameof(capabilitys));

            foreach (string capability in capabilitys)
            {
                WithCapAdd(capability);
            }

            return this;
        }

        /// <summary>
        /// Sets: the Linux capabilities to drop, for example <c>ALL</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cap_drop"/>
        /// </summary>
        /// <param name="capability">The capability to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCapDrop(string capability)
        {
            Guard.NotNullOrWhiteSpace(capability, nameof(capability));
            _definition = _definition with { CapDrop = Collections.Append(_definition.CapDrop, capability) };
            return this;
        }

        /// <summary>
        /// Sets: the Linux capabilities to drop, for example <c>ALL</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cap_drop"/>
        /// </summary>
        /// <param name="capabilitys">The capabilitys to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCapDrop(IEnumerable<string> capabilitys)
        {
            Guard.NotNull(capabilitys, nameof(capabilitys));

            foreach (string capability in capabilitys)
            {
                WithCapDrop(capability);
            }

            return this;
        }

        /// <summary>
        /// Sets: the security options for the container, for example <c>no-new-privileges:true</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#security_opt"/>
        /// </summary>
        /// <param name="option">The option to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecurityOpt(string option)
        {
            Guard.NotNullOrWhiteSpace(option, nameof(option));
            _definition = _definition with { SecurityOpt = Collections.Append(_definition.SecurityOpt, option) };
            return this;
        }

        /// <summary>
        /// Sets: the security options for the container, for example <c>no-new-privileges:true</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#security_opt"/>
        /// </summary>
        /// <param name="options">The options to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSecurityOpt(IEnumerable<string> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (string option in options)
            {
                WithSecurityOpt(option);
            }

            return this;
        }

        /// <summary>
        /// Sets: the additional groups (names or numeric IDs) the container user joins.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#group_add"/>
        /// </summary>
        /// <param name="group">The group to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithGroupAdd(string group)
        {
            Guard.NotNullOrWhiteSpace(group, nameof(group));
            _definition = _definition with { GroupAdd = Collections.Append(_definition.GroupAdd, group) };
            return this;
        }

        /// <summary>
        /// Sets: the additional groups (names or numeric IDs) the container user joins.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#group_add"/>
        /// </summary>
        /// <param name="groups">The groups to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithGroupAdd(IEnumerable<string> groups)
        {
            Guard.NotNull(groups, nameof(groups));

            foreach (string group in groups)
            {
                WithGroupAdd(group);
            }

            return this;
        }

        /// <summary>
        /// Sets: the device cgroup rules, for example <c>c 1:3 mr</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#device_cgroup_rules"/>
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDeviceCgroupRules(string rule)
        {
            Guard.NotNullOrWhiteSpace(rule, nameof(rule));
            _definition = _definition with { DeviceCgroupRules = Collections.Append(_definition.DeviceCgroupRules, rule) };
            return this;
        }

        /// <summary>
        /// Sets: the device cgroup rules, for example <c>c 1:3 mr</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#device_cgroup_rules"/>
        /// </summary>
        /// <param name="rules">The rules to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDeviceCgroupRules(IEnumerable<string> rules)
        {
            Guard.NotNull(rules, nameof(rules));

            foreach (string rule in rules)
            {
                WithDeviceCgroupRules(rule);
            }

            return this;
        }

        /// <summary>
        /// Sets: the custom DNS servers.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#dns"/>
        /// </summary>
        /// <param name="server">The server to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDns(string server)
        {
            Guard.NotNullOrWhiteSpace(server, nameof(server));
            _definition = _definition with { Dns = Collections.Append(_definition.Dns, server) };
            return this;
        }

        /// <summary>
        /// Sets: the custom DNS servers.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#dns"/>
        /// </summary>
        /// <param name="servers">The servers to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDns(IEnumerable<string> servers)
        {
            Guard.NotNull(servers, nameof(servers));

            foreach (string server in servers)
            {
                WithDns(server);
            }

            return this;
        }

        /// <summary>
        /// Sets: the custom DNS options.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#dns_opt"/>
        /// </summary>
        /// <param name="option">The option to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDnsOpt(string option)
        {
            Guard.NotNullOrWhiteSpace(option, nameof(option));
            _definition = _definition with { DnsOpt = Collections.Append(_definition.DnsOpt, option) };
            return this;
        }

        /// <summary>
        /// Sets: the custom DNS options.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#dns_opt"/>
        /// </summary>
        /// <param name="options">The options to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDnsOpt(IEnumerable<string> options)
        {
            Guard.NotNull(options, nameof(options));

            foreach (string option in options)
            {
                WithDnsOpt(option);
            }

            return this;
        }

        /// <summary>
        /// Sets: the custom DNS search domains.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#dns_search"/>
        /// </summary>
        /// <param name="domain">The domain to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDnsSearch(string domain)
        {
            Guard.NotNullOrWhiteSpace(domain, nameof(domain));
            _definition = _definition with { DnsSearch = Collections.Append(_definition.DnsSearch, domain) };
            return this;
        }

        /// <summary>
        /// Sets: the custom DNS search domains.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#dns_search"/>
        /// </summary>
        /// <param name="domains">The domains to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDnsSearch(IEnumerable<string> domains)
        {
            Guard.NotNull(domains, nameof(domains));

            foreach (string domain in domains)
            {
                WithDnsSearch(domain);
            }

            return this;
        }

        /// <summary>
        /// Sets: the ports exposed to linked services without publishing them to the host, for example <c>3000</c> or <c>8000-8010</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#expose"/>
        /// </summary>
        /// <param name="port">The port to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExpose(string port)
        {
            Guard.NotNullOrWhiteSpace(port, nameof(port));
            _definition = _definition with { Expose = Collections.Append(_definition.Expose, port) };
            return this;
        }

        /// <summary>
        /// Sets: the ports exposed to linked services without publishing them to the host, for example <c>3000</c> or <c>8000-8010</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#expose"/>
        /// </summary>
        /// <param name="ports">The ports to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExpose(IEnumerable<string> ports)
        {
            Guard.NotNull(ports, nameof(ports));

            foreach (string port in ports)
            {
                WithExpose(port);
            }

            return this;
        }

        /// <summary>
        /// Sets: the links to other services, as <c>service</c> or <c>service:alias</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#links"/>
        /// </summary>
        /// <param name="link">The link to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLinks(string link)
        {
            Guard.NotNullOrWhiteSpace(link, nameof(link));
            _definition = _definition with { Links = Collections.Append(_definition.Links, link) };
            return this;
        }

        /// <summary>
        /// Sets: the links to other services, as <c>service</c> or <c>service:alias</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#links"/>
        /// </summary>
        /// <param name="links">The links to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithLinks(IEnumerable<string> links)
        {
            Guard.NotNull(links, nameof(links));

            foreach (string link in links)
            {
                WithLinks(link);
            }

            return this;
        }

        /// <summary>
        /// Sets: the links to containers started outside this file, as <c>container</c> or <c>container:alias</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#external_links"/>
        /// </summary>
        /// <param name="link">The link to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExternalLinks(string link)
        {
            Guard.NotNullOrWhiteSpace(link, nameof(link));
            _definition = _definition with { ExternalLinks = Collections.Append(_definition.ExternalLinks, link) };
            return this;
        }

        /// <summary>
        /// Sets: the links to containers started outside this file, as <c>container</c> or <c>container:alias</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#external_links"/>
        /// </summary>
        /// <param name="links">The links to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExternalLinks(IEnumerable<string> links)
        {
            Guard.NotNull(links, nameof(links));

            foreach (string link in links)
            {
                WithExternalLinks(link);
            }

            return this;
        }

        /// <summary>
        /// Sets: the services or containers to mount all volumes from, as <c>service</c>, <c>service:ro</c> or <c>container:name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes_from"/>
        /// </summary>
        /// <param name="source">The source to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolumesFrom(string source)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            _definition = _definition with { VolumesFrom = Collections.Append(_definition.VolumesFrom, source) };
            return this;
        }

        /// <summary>
        /// Sets: the services or containers to mount all volumes from, as <c>service</c>, <c>service:ro</c> or <c>container:name</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#volumes_from"/>
        /// </summary>
        /// <param name="sources">The sources to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithVolumesFrom(IEnumerable<string> sources)
        {
            Guard.NotNull(sources, nameof(sources));

            foreach (string source in sources)
            {
                WithVolumesFrom(source);
            }

            return this;
        }

        /// <summary>
        /// Sets: the temporary filesystems to mount, as paths with optional options.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#tmpfs"/>
        /// </summary>
        /// <param name="mount">The mount to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithTmpfs(string mount)
        {
            Guard.NotNullOrWhiteSpace(mount, nameof(mount));
            _definition = _definition with { Tmpfs = Collections.Append(_definition.Tmpfs, mount) };
            return this;
        }

        /// <summary>
        /// Sets: the temporary filesystems to mount, as paths with optional options.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#tmpfs"/>
        /// </summary>
        /// <param name="mounts">The mounts to add.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithTmpfs(IEnumerable<string> mounts)
        {
            Guard.NotNull(mounts, nameof(mounts));

            foreach (string mount in mounts)
            {
                WithTmpfs(mount);
            }

            return this;
        }

        /// <summary>
        /// Sets: the number of CPUs the container can use, for example <c>0.5</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpus"/>
        /// </summary>
        /// <param name="cpus">The CPU count, for example <c>0.5</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpus(string cpus)
        {
            Guard.NotNullOrWhiteSpace(cpus, nameof(cpus));
            _definition = _definition with { Cpus = cpus };
            return this;
        }

        /// <summary>
        /// Sets: the number of CPUs the container can use, for example <c>0.5</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpus"/>
        /// </summary>
        /// <param name="cpus">The CPU count, which must be positive.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpus(double cpus)
        {
            if (cpus <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpus), cpus, "The CPU count must be positive.");
            }

            return WithCpus(cpus.ToString("0.###", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets: the number of usable CPUs (Windows).
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_count"/>
        /// </summary>
        /// <param name="cpuCount">The value, at least 0.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuCount(int cpuCount)
        {
            if (cpuCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuCount), cpuCount, "The value must be at least 0.");
            }

            _definition = _definition with { CpuCount = cpuCount };
            return this;
        }

        /// <summary>
        /// Sets: the percentage of usable CPU (Windows), from 0 to 100.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_percent"/>
        /// </summary>
        /// <param name="cpuPercent">The value, between 0 and 100.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuPercent(int cpuPercent)
        {
            if (cpuPercent < 0 || cpuPercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuPercent), cpuPercent, "The value must be between 0 and 100.");
            }

            _definition = _definition with { CpuPercent = cpuPercent };
            return this;
        }

        /// <summary>
        /// Sets: the relative CPU weight.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_shares"/>
        /// </summary>
        /// <param name="cpuShares">The value, at least 0.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuShares(long cpuShares)
        {
            if (cpuShares < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuShares), cpuShares, "The value must be at least 0.");
            }

            _definition = _definition with { CpuShares = cpuShares };
            return this;
        }

        /// <summary>
        /// Sets: the CFS CPU quota in microseconds.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_quota"/>
        /// </summary>
        /// <param name="cpuQuota">The value, at least 0.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuQuota(long cpuQuota)
        {
            if (cpuQuota < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuQuota), cpuQuota, "The value must be at least 0.");
            }

            _definition = _definition with { CpuQuota = cpuQuota };
            return this;
        }

        /// <summary>
        /// Sets: the CFS CPU period in microseconds.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_period"/>
        /// </summary>
        /// <param name="cpuPeriod">The value, at least 0.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuPeriod(long cpuPeriod)
        {
            if (cpuPeriod < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuPeriod), cpuPeriod, "The value must be at least 0.");
            }

            _definition = _definition with { CpuPeriod = cpuPeriod };
            return this;
        }

        /// <summary>
        /// Sets: the CPU real-time period in microseconds.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_rt_period"/>
        /// </summary>
        /// <param name="cpuRtPeriod">The value, at least 0.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuRtPeriod(long cpuRtPeriod)
        {
            if (cpuRtPeriod < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuRtPeriod), cpuRtPeriod, "The value must be at least 0.");
            }

            _definition = _definition with { CpuRtPeriod = cpuRtPeriod };
            return this;
        }

        /// <summary>
        /// Sets: the CPU real-time runtime in microseconds.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpu_rt_runtime"/>
        /// </summary>
        /// <param name="cpuRtRuntime">The value, at least 0.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuRtRuntime(long cpuRtRuntime)
        {
            if (cpuRtRuntime < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cpuRtRuntime), cpuRtRuntime, "The value must be at least 0.");
            }

            _definition = _definition with { CpuRtRuntime = cpuRtRuntime };
            return this;
        }

        /// <summary>
        /// Sets: the CPUs the container may run on, for example <c>0-3</c> or <c>0,1</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#cpuset"/>
        /// </summary>
        /// <param name="cpuset">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCpuset(string cpuset)
        {
            Guard.NotNullOrWhiteSpace(cpuset, nameof(cpuset));
            _definition = _definition with { Cpuset = cpuset };
            return this;
        }

        /// <summary>
        /// Sets: the memory limit, as a compose-spec byte value such as <c>512m</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#mem_limit"/>
        /// </summary>
        /// <param name="memLimit">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithMemLimit(string memLimit)
        {
            Guard.NotNullOrWhiteSpace(memLimit, nameof(memLimit));
            _definition = _definition with { MemLimit = memLimit };
            return this;
        }

        /// <summary>
        /// Sets: the soft memory limit, as a compose-spec byte value such as <c>256m</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#mem_reservation"/>
        /// </summary>
        /// <param name="memReservation">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithMemReservation(string memReservation)
        {
            Guard.NotNullOrWhiteSpace(memReservation, nameof(memReservation));
            _definition = _definition with { MemReservation = memReservation };
            return this;
        }

        /// <summary>
        /// Sets: the memory plus swap limit, as a compose-spec byte value; <c>-1</c> is unlimited swap.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#memswap_limit"/>
        /// </summary>
        /// <param name="memswapLimit">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithMemswapLimit(string memswapLimit)
        {
            Guard.NotNullOrWhiteSpace(memswapLimit, nameof(memswapLimit));
            _definition = _definition with { MemswapLimit = memswapLimit };
            return this;
        }

        /// <summary>
        /// Sets: how readily the kernel swaps container memory, from 0 to 100.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#mem_swappiness"/>
        /// </summary>
        /// <param name="memSwappiness">The value, between 0 and 100.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithMemSwappiness(int memSwappiness)
        {
            if (memSwappiness < 0 || memSwappiness > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(memSwappiness), memSwappiness, "The value must be between 0 and 100.");
            }

            _definition = _definition with { MemSwappiness = memSwappiness };
            return this;
        }

        /// <summary>
        /// Sets: whether to disable the out-of-memory killer for the container.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#oom_kill_disable"/>
        /// </summary>
        /// <param name="oomKillDisable"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithOomKillDisable(bool oomKillDisable)
        {
            _definition = _definition with { OomKillDisable = oomKillDisable };
            return this;
        }

        /// <summary>
        /// Sets: the out-of-memory preference, from -1000 to 1000.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#oom_score_adj"/>
        /// </summary>
        /// <param name="oomScoreAdj">The value, between -1000 and 1000.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithOomScoreAdj(int oomScoreAdj)
        {
            if (oomScoreAdj < -1000 || oomScoreAdj > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(oomScoreAdj), oomScoreAdj, "The value must be between -1000 and 1000.");
            }

            _definition = _definition with { OomScoreAdj = oomScoreAdj };
            return this;
        }

        /// <summary>
        /// Sets: the maximum number of processes, or <c>-1</c> for unlimited.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pids_limit"/>
        /// </summary>
        /// <param name="pidsLimit">The value, at least -1.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPidsLimit(int pidsLimit)
        {
            if (pidsLimit < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(pidsLimit), pidsLimit, "The value must be at least -1.");
            }

            _definition = _definition with { PidsLimit = pidsLimit };
            return this;
        }

        /// <summary>
        /// Sets: the size of <c>/dev/shm</c>, as a compose-spec byte value such as <c>64m</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#shm_size"/>
        /// </summary>
        /// <param name="shmSize">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithShmSize(string shmSize)
        {
            Guard.NotNullOrWhiteSpace(shmSize, nameof(shmSize));
            _definition = _definition with { ShmSize = shmSize };
            return this;
        }

        /// <summary>
        /// Adds an exposed port.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#expose"/>
        /// </summary>
        /// <param name="port">The port, from 1 to 65535.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExpose(int port)
        {
            Guard.ValidPort(port, nameof(port));
            return WithExpose(port.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the block I/O configuration, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
        /// </summary>
        /// <param name="blkioConfig">The block I/O configuration.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithBlkioConfig(BlkioConfigDefinition blkioConfig)
        {
            Guard.NotNull(blkioConfig, nameof(blkioConfig));
            _definition = _definition with { BlkioConfig = blkioConfig };
            return this;
        }

        /// <summary>
        /// Sets the block I/O configuration, configured through a <see cref="BlkioConfigBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#blkio_config"/>
        /// </summary>
        /// <param name="configure">Configures the block I/O configuration.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithBlkioConfig(Action<BlkioConfigBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            BlkioConfigBuilder builder = new BlkioConfigBuilder();
            configure(builder);
            return WithBlkioConfig(builder.Build());
        }

        /// <summary>
        /// Sets the credential spec, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#credential_spec"/>
        /// </summary>
        /// <param name="credentialSpec">The credential spec.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCredentialSpec(CredentialSpecDefinition credentialSpec)
        {
            Guard.NotNull(credentialSpec, nameof(credentialSpec));
            _definition = _definition with { CredentialSpec = credentialSpec };
            return this;
        }

        /// <summary>
        /// Sets the credential spec, configured through a <see cref="CredentialSpecBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#credential_spec"/>
        /// </summary>
        /// <param name="configure">Configures the credential spec.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithCredentialSpec(Action<CredentialSpecBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            CredentialSpecBuilder builder = new CredentialSpecBuilder();
            configure(builder);
            return WithCredentialSpec(builder.Build());
        }

        /// <summary>
        /// Makes a host device available in the container at the same path.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#devices"/>
        /// </summary>
        /// <param name="source">The host device path, for example <c>/dev/ttyUSB0</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDevice(string source)
        {
            return WithDevice(new DeviceMappingBuilder().WithSource(source).Build());
        }

        /// <summary>
        /// Makes a host device available in the container at a different path.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#devices"/>
        /// </summary>
        /// <param name="source">The host device path.</param>
        /// <param name="target">The path in the container.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDevice(string source, string target)
        {
            return WithDevice(new DeviceMappingBuilder().WithSource(source).WithTarget(target).Build());
        }

        /// <summary>
        /// Makes a host device available in the container, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#devices"/>
        /// </summary>
        /// <param name="device">The device mapping.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDevice(DeviceMappingDefinition device)
        {
            Guard.NotNull(device, nameof(device));
            _definition = _definition with { Devices = Collections.Append(_definition.Devices, device) };
            return this;
        }

        /// <summary>
        /// Makes a host device available in the container, configured through a <see cref="DeviceMappingBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#devices"/>
        /// </summary>
        /// <param name="configure">Configures the device mapping.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithDevice(Action<DeviceMappingBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            DeviceMappingBuilder builder = new DeviceMappingBuilder();
            configure(builder);
            return WithDevice(builder.Build());
        }

        /// <summary>
        /// Adds an additional hostname to the container's <c>/etc/hosts</c>. Setting the same hostname again replaces its address.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#extra_hosts"/>
        /// </summary>
        /// <param name="host">The hostname.</param>
        /// <param name="address">The IP address, or <c>host-gateway</c>.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExtraHost(string host, string address)
        {
            Guard.NotNullOrWhiteSpace(host, nameof(host));
            Guard.NotNullOrWhiteSpace(address, nameof(address));
            _definition = _definition with { ExtraHosts = Collections.With(_definition.ExtraHosts, host, address) };
            return this;
        }

        /// <summary>
        /// Lets the container use every available GPU. Replaces any specific GPU requests.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#gpus"/>
        /// </summary>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithAllGpus()
        {
            _definition = _definition with { Gpus = new GpusDefinition { All = true } };
            return this;
        }

        /// <summary>
        /// Adds a request for specific GPUs, from an existing definition. Replaces a previous request for all GPUs.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#gpus"/>
        /// </summary>
        /// <param name="gpu">The GPU request.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithGpu(GpuDefinition gpu)
        {
            Guard.NotNull(gpu, nameof(gpu));

            IReadOnlyList<GpuDefinition> existing = _definition.Gpus != null && !_definition.Gpus.All ? _definition.Gpus.Devices : System.Array.Empty<GpuDefinition>();
            _definition = _definition with { Gpus = new GpusDefinition { Devices = Collections.Append(existing, gpu) } };
            return this;
        }

        /// <summary>
        /// Adds a request for specific GPUs, configured through a <see cref="GpuBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#gpus"/>
        /// </summary>
        /// <param name="configure">Configures the GPU request.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithGpu(Action<GpuBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            GpuBuilder builder = new GpuBuilder();
            configure(builder);
            return WithGpu(builder.Build());
        }

        /// <summary>
        /// Sets a storage driver option. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#storage_opt"/>
        /// </summary>
        /// <param name="key">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithStorageOpt(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));
            _definition = _definition with { StorageOpt = Collections.With(_definition.StorageOpt, key, value) };
            return this;
        }

        /// <summary>
        /// Sets a kernel parameter. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#sysctls"/>
        /// </summary>
        /// <param name="key">The parameter name, for example <c>net.core.somaxconn</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSysctl(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));
            _definition = _definition with { Sysctls = Collections.With(_definition.Sysctls, key, value) };
            return this;
        }

        /// <summary>
        /// Sets a numeric kernel parameter. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#sysctls"/>
        /// </summary>
        /// <param name="key">The parameter name, for example <c>net.core.somaxconn</c>.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithSysctl(string key, int value)
        {
            return WithSysctl(key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets a resource limit with one value for both the soft and hard limit.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, such as <c>nproc</c> or <c>core</c>.</param>
        /// <param name="limit">The limit. <c>-1</c> means unlimited for most limits.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUlimit(string name, int limit)
        {
            return WithUlimit(name, new UlimitBuilder().WithLimit(limit).Build());
        }

        /// <summary>
        /// Sets a resource limit with separate soft and hard limits.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, such as <c>nofile</c>.</param>
        /// <param name="soft">The soft limit, the value actually enforced.</param>
        /// <param name="hard">The hard limit, the maximum allowed value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUlimit(string name, int soft, int hard)
        {
            return WithUlimit(name, new UlimitBuilder().WithSoft(soft).WithHard(hard).Build());
        }

        /// <summary>
        /// Sets a resource limit, from an existing definition. Setting the same name again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, which must be lower-case letters, such as <c>nofile</c>.</param>
        /// <param name="ulimit">The limit definition.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUlimit(string name, UlimitDefinition ulimit)
        {
            Guard.UlimitName(name, nameof(name));
            Guard.NotNull(ulimit, nameof(ulimit));
            _definition = _definition with { Ulimits = Collections.With(_definition.Ulimits, name, ulimit) };
            return this;
        }

        /// <summary>
        /// Sets a resource limit, configured through a <see cref="UlimitBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, which must be lower-case letters, such as <c>nofile</c>.</param>
        /// <param name="configure">Configures the limit.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithUlimit(string name, Action<UlimitBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            UlimitBuilder builder = new UlimitBuilder();
            configure(builder);
            return WithUlimit(name, builder.Build());
        }

        /// <summary>
        /// Sets an annotation on the container. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#annotations"/>
        /// </summary>
        /// <param name="key">The annotation key.</param>
        /// <param name="value">The annotation value.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithAnnotation(string key, string value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));
            Guard.NotNull(value, nameof(value));
            _definition = _definition with { Annotations = Collections.With(_definition.Annotations, key, value) };
            return this;
        }

        /// <summary>
        /// Adds a command to run after the container starts, as a shell-form string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#post_start"/>
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPostStart(string command)
        {
            return WithPostStart(new ServiceHookBuilder().WithCommand(command).Build());
        }

        /// <summary>
        /// Adds a command to run after the container starts, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#post_start"/>
        /// </summary>
        /// <param name="hook">The hook.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPostStart(ServiceHookDefinition hook)
        {
            Guard.NotNull(hook, nameof(hook));
            _definition = _definition with { PostStart = Collections.Append(_definition.PostStart, hook) };
            return this;
        }

        /// <summary>
        /// Adds a command to run after the container starts, configured through a <see cref="ServiceHookBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#post_start"/>
        /// </summary>
        /// <param name="configure">Configures the hook.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPostStart(Action<ServiceHookBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ServiceHookBuilder builder = new ServiceHookBuilder();
            configure(builder);
            return WithPostStart(builder.Build());
        }

        /// <summary>
        /// Adds a command to run before the container stops, as a shell-form string.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_stop"/>
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPreStop(string command)
        {
            return WithPreStop(new ServiceHookBuilder().WithCommand(command).Build());
        }

        /// <summary>
        /// Adds a command to run before the container stops, from an existing definition.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_stop"/>
        /// </summary>
        /// <param name="hook">The hook.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPreStop(ServiceHookDefinition hook)
        {
            Guard.NotNull(hook, nameof(hook));
            _definition = _definition with { PreStop = Collections.Append(_definition.PreStop, hook) };
            return this;
        }

        /// <summary>
        /// Adds a command to run before the container stops, configured through a <see cref="ServiceHookBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#pre_stop"/>
        /// </summary>
        /// <param name="configure">Configures the hook.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithPreStop(Action<ServiceHookBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            ServiceHookBuilder builder = new ServiceHookBuilder();
            configure(builder);
            return WithPreStop(builder.Build());
        }

        /// <summary>
        /// Sets an extension field on this service. Setting the same key again replaces its value. Compose
        /// ignores these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public ServiceBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the service definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="ServiceDefinition"/>.</returns>
        public ServiceDefinition Build()
        {
            return _definition;
        }

        private ServiceBuilder AddPort(PortMapping port)
        {
            List<PortMapping> ports = new List<PortMapping>(_definition.Ports) { port };
            _definition = _definition with { Ports = ports.AsReadOnly() };
            return this;
        }

        private ServiceBuilder AddVolume(MountMapping mount)
        {
            List<MountMapping> volumes = new List<MountMapping>(_definition.Volumes) { mount };
            _definition = _definition with { Volumes = volumes.AsReadOnly() };
            return this;
        }

        private ServiceBuilder AddConfig(ConfigReference config)
        {
            _definition = _definition with { Configs = Collections.Append(_definition.Configs, config) };
            return this;
        }

        private ServiceBuilder AddSecret(SecretReference secret)
        {
            _definition = _definition with { Secrets = Collections.Append(_definition.Secrets, secret) };
            return this;
        }

        private ServiceBuilder AddEnvFile(EnvFileEntry entry)
        {
            Guard.NotNullOrWhiteSpace(entry.Path, "Path");
            _definition = _definition with { EnvFiles = Collections.Append(_definition.EnvFiles, entry) };
            return this;
        }

        private ServiceBuilder AddDependency(string service, DependencyDefinition dependency)
        {
            Guard.NotNullOrWhiteSpace(service, nameof(service));

            _definition = _definition with { DependsOn = Collections.With(_definition.DependsOn, service, dependency) };
            return this;
        }

        private ServiceBuilder AddNetwork(string name, NetworkAttachment attachment)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));

            _definition = _definition with { Networks = Collections.With(_definition.Networks, name, attachment) };
            return this;
        }

        private ServiceBuilder SetEnvironment(string key, string? value, string parameterName)
        {
            Guard.EnvironmentKey(key, parameterName);

            EnvironmentVariables environment = _definition.Environment;
            _definition = _definition with
            {
                Environment = environment with { Variables = Collections.With(environment.Variables, key, value) }
            };
            return this;
        }
    }
}
