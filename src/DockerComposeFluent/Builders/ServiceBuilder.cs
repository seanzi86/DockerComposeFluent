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
