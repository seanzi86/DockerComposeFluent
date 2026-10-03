using System;
using System.Collections.Generic;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Builders
{
    /// <summary>
    /// Fluently builds a <see cref="BuildDefinition"/>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md"/>
    /// </summary>
    public sealed class BuildBuilder
    {
        private BuildDefinition _definition = new BuildDefinition();

        /// <summary>
        /// Sets: the additional named build contexts, keyed by name with the location as the value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#additional_contexts"/>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithAdditionalContext(string name, string location)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(location, nameof(location));
            _definition = _definition with { AdditionalContexts = Collections.With(_definition.AdditionalContexts, name, location) };
            return this;
        }

        /// <summary>
        /// Sets: the build arguments, keyed by name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#args"/>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithArg(string name, string value)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNull(value, nameof(value));
            _definition = _definition with { Args = Collections.With(_definition.Args, name, value) };
            return this;
        }

        /// <summary>
        /// Sets: the sources to use as the image cache.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#cache_from"/>
        /// </summary>
        /// <param name="source">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithCacheFrom(string source)
        {
            Guard.NotNullOrWhiteSpace(source, nameof(source));
            _definition = _definition with { CacheFrom = Collections.Append(_definition.CacheFrom, source) };
            return this;
        }

        /// <summary>
        /// Sets: the sources to use as the image cache.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#cache_from"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithCacheFrom(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithCacheFrom(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: the locations to export the build cache to.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#cache_to"/>
        /// </summary>
        /// <param name="destination">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithCacheTo(string destination)
        {
            Guard.NotNullOrWhiteSpace(destination, nameof(destination));
            _definition = _definition with { CacheTo = Collections.Append(_definition.CacheTo, destination) };
            return this;
        }

        /// <summary>
        /// Sets: the locations to export the build cache to.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#cache_to"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithCacheTo(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithCacheTo(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: the path to the build context: a directory containing a Dockerfile, or a git repository URL.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#context"/>
        /// </summary>
        /// <param name="context">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithContext(string context)
        {
            Guard.NotNullOrWhiteSpace(context, nameof(context));
            _definition = _definition with { Context = context };
            return this;
        }

        /// <summary>
        /// Sets: the Dockerfile to build from, relative to the context.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#dockerfile"/>
        /// </summary>
        /// <param name="dockerfile">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithDockerfile(string dockerfile)
        {
            Guard.NotNullOrWhiteSpace(dockerfile, nameof(dockerfile));
            _definition = _definition with { Dockerfile = dockerfile };
            return this;
        }

        /// <summary>
        /// Sets: the Dockerfile contents, defined inline.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#dockerfile_inline"/>
        /// </summary>
        /// <param name="dockerfileInline">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithDockerfileInline(string dockerfileInline)
        {
            Guard.NotNullOrWhiteSpace(dockerfileInline, nameof(dockerfileInline));
            _definition = _definition with { DockerfileInline = dockerfileInline };
            return this;
        }

        /// <summary>
        /// Sets: the extra privileged entitlements the build is granted, for example <c>network.host</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#entitlements"/>
        /// </summary>
        /// <param name="entitlement">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithEntitlements(string entitlement)
        {
            Guard.NotNullOrWhiteSpace(entitlement, nameof(entitlement));
            _definition = _definition with { Entitlements = Collections.Append(_definition.Entitlements, entitlement) };
            return this;
        }

        /// <summary>
        /// Sets: the extra privileged entitlements the build is granted, for example <c>network.host</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#entitlements"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithEntitlements(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithEntitlements(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: the container isolation technology used for the build; supported values are platform-specific.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#isolation"/>
        /// </summary>
        /// <param name="isolation">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithIsolation(string isolation)
        {
            Guard.NotNullOrWhiteSpace(isolation, nameof(isolation));
            _definition = _definition with { Isolation = isolation };
            return this;
        }

        /// <summary>
        /// Sets: the network the build containers use during <c>RUN</c> instructions.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#network"/>
        /// </summary>
        /// <param name="network">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithNetwork(string network)
        {
            Guard.NotNullOrWhiteSpace(network, nameof(network));
            _definition = _definition with { Network = network };
            return this;
        }

        /// <summary>
        /// Sets: whether to build without using the cache.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#no_cache"/>
        /// </summary>
        /// <param name="noCache"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithNoCache(bool noCache)
        {
            _definition = _definition with { NoCache = noCache };
            return this;
        }

        /// <summary>
        /// Sets: the stages that are built without the cache.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#no_cache"/>
        /// </summary>
        /// <param name="stage">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithNoCacheFilter(string stage)
        {
            Guard.NotNullOrWhiteSpace(stage, nameof(stage));
            _definition = _definition with { NoCacheFilter = Collections.Append(_definition.NoCacheFilter, stage) };
            return this;
        }

        /// <summary>
        /// Sets: the stages that are built without the cache.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#no_cache"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithNoCacheFilter(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithNoCacheFilter(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: the platforms to build the image for, for example <c>linux/amd64</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#platforms"/>
        /// </summary>
        /// <param name="platform">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithPlatforms(string platform)
        {
            Guard.NotNullOrWhiteSpace(platform, nameof(platform));
            _definition = _definition with { Platforms = Collections.Append(_definition.Platforms, platform) };
            return this;
        }

        /// <summary>
        /// Sets: the platforms to build the image for, for example <c>linux/amd64</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#platforms"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithPlatforms(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithPlatforms(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: whether the build runs with extended privileges.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#privileged"/>
        /// </summary>
        /// <param name="privileged"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithPrivileged(bool privileged)
        {
            _definition = _definition with { Privileged = privileged };
            return this;
        }

        /// <summary>
        /// Sets: the provenance attestation to add to the image: <c>true</c>, <c>false</c> or a setting such as <c>mode=max</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#provenance"/>
        /// </summary>
        /// <param name="provenance"><c>true</c> or <c>false</c>.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithProvenance(bool provenance)
        {
            return WithProvenance(provenance ? "true" : "false");
        }

        /// <summary>
        /// Sets: the provenance attestation to add to the image: <c>true</c>, <c>false</c> or a setting such as <c>mode=max</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#provenance"/>
        /// </summary>
        /// <param name="provenance">The setting, for example <c>mode=max</c>.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithProvenance(string provenance)
        {
            Guard.NotNullOrWhiteSpace(provenance, nameof(provenance));
            _definition = _definition with { Provenance = provenance };
            return this;
        }

        /// <summary>
        /// Sets: whether to always pull newer versions of the base images.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#pull"/>
        /// </summary>
        /// <param name="pull"><c>true</c> to enable it.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithPull(bool pull)
        {
            _definition = _definition with { Pull = pull };
            return this;
        }

        /// <summary>
        /// Sets: the SBOM attestation to add to the image: <c>true</c>, <c>false</c> or a setting.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#sbom"/>
        /// </summary>
        /// <param name="sbom"><c>true</c> or <c>false</c>.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSbom(bool sbom)
        {
            return WithSbom(sbom ? "true" : "false");
        }

        /// <summary>
        /// Sets: the SBOM attestation to add to the image: <c>true</c>, <c>false</c> or a setting.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#sbom"/>
        /// </summary>
        /// <param name="sbom">The setting, for example <c>mode=max</c>.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSbom(string sbom)
        {
            Guard.NotNullOrWhiteSpace(sbom, nameof(sbom));
            _definition = _definition with { Sbom = sbom };
            return this;
        }

        /// <summary>
        /// Sets: the size of <c>/dev/shm</c> for the build, as a compose-spec byte value such as <c>64m</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#shm_size"/>
        /// </summary>
        /// <param name="shmSize">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithShmSize(string shmSize)
        {
            Guard.NotNullOrWhiteSpace(shmSize, nameof(shmSize));
            _definition = _definition with { ShmSize = shmSize };
            return this;
        }

        /// <summary>
        /// Sets: the SSH agent sockets or keys the build can use, as <c>default</c> or <c>id=path</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ssh"/>
        /// </summary>
        /// <param name="ssh">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSsh(string ssh)
        {
            Guard.NotNullOrWhiteSpace(ssh, nameof(ssh));
            _definition = _definition with { Ssh = Collections.Append(_definition.Ssh, ssh) };
            return this;
        }

        /// <summary>
        /// Sets: the SSH agent sockets or keys the build can use, as <c>default</c> or <c>id=path</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ssh"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSsh(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithSsh(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: the extra tags to apply to the built image.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#tags"/>
        /// </summary>
        /// <param name="tag">The value to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithTags(string tag)
        {
            Guard.NotNullOrWhiteSpace(tag, nameof(tag));
            _definition = _definition with { Tags = Collections.Append(_definition.Tags, tag) };
            return this;
        }

        /// <summary>
        /// Sets: the extra tags to apply to the built image.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#tags"/>
        /// </summary>
        /// <param name="values">The values to add.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithTags(IEnumerable<string> values)
        {
            Guard.NotNull(values, nameof(values));

            foreach (string value in values)
            {
                WithTags(value);
            }

            return this;
        }

        /// <summary>
        /// Sets: the Dockerfile stage to build.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#target"/>
        /// </summary>
        /// <param name="target">The value.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithTarget(string target)
        {
            Guard.NotNullOrWhiteSpace(target, nameof(target));
            _definition = _definition with { Target = target };
            return this;
        }

        /// <summary>
        /// Sets a label on the built image, keeping the mapping form. Setting the same key again replaces its value.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#labels"/>
        /// </summary>
        /// <param name="key">The label key, which must not start with the reserved <c>com.docker.compose</c> prefix.</param>
        /// <param name="value">The label value, which may be empty.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithLabel(string key, string value)
        {
            _definition = _definition with { Labels = LabelsMutator.Set(_definition.Labels, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Sets several labels from key/value pairs, keeping the mapping form.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#labels"/>
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithLabels(IEnumerable<KeyValuePair<string, string>> labels)
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
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#labels"/>
        /// </summary>
        /// <param name="entries">The entries, split at the first <c>=</c>.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithLabels(IEnumerable<string> entries)
        {
            _definition = _definition with { Labels = LabelsMutator.SetFromEntries(_definition.Labels, entries, nameof(entries)) };
            return this;
        }

        /// <summary>
        /// Adds an additional hostname to the build containers' <c>/etc/hosts</c>. Setting the same hostname again replaces its address.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#extra_hosts"/>
        /// </summary>
        /// <param name="host">The hostname.</param>
        /// <param name="address">The IP address, or <c>host-gateway</c>.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithExtraHost(string host, string address)
        {
            Guard.NotNullOrWhiteSpace(host, nameof(host));
            Guard.NotNullOrWhiteSpace(address, nameof(address));
            _definition = _definition with { ExtraHosts = Collections.With(_definition.ExtraHosts, host, address) };
            return this;
        }

        /// <summary>
        /// Grants the build access to a secret, by name.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#secrets"/>
        /// </summary>
        /// <param name="source">The name of the secret as defined in the top-level <c>secrets</c> section.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSecret(string source)
        {
            return WithSecret(new SecretReferenceBuilder().WithSource(source).Build());
        }

        /// <summary>
        /// Grants the build access to a secret, from an existing reference.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#secrets"/>
        /// </summary>
        /// <param name="secret">The secret reference.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSecret(SecretReference secret)
        {
            Guard.NotNull(secret, nameof(secret));
            _definition = _definition with { Secrets = Collections.Append(_definition.Secrets, secret) };
            return this;
        }

        /// <summary>
        /// Grants the build access to a secret, configured through a <see cref="SecretReferenceBuilder"/>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#secrets"/>
        /// </summary>
        /// <param name="configure">Configures the reference.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithSecret(Action<SecretReferenceBuilder> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            SecretReferenceBuilder builder = new SecretReferenceBuilder();
            configure(builder);
            return WithSecret(builder.Build());
        }

        /// <summary>
        /// Sets a resource limit with one value for both the soft and hard limit.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, such as <c>nproc</c>.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithUlimit(string name, int limit)
        {
            return WithUlimit(name, new UlimitBuilder().WithLimit(limit).Build());
        }

        /// <summary>
        /// Sets a resource limit with separate soft and hard limits.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, such as <c>nofile</c>.</param>
        /// <param name="soft">The soft limit.</param>
        /// <param name="hard">The hard limit.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithUlimit(string name, int soft, int hard)
        {
            return WithUlimit(name, new UlimitBuilder().WithSoft(soft).WithHard(hard).Build());
        }

        /// <summary>
        /// Sets a resource limit, from an existing definition. Setting the same name again replaces it.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ulimits"/>
        /// </summary>
        /// <param name="name">The limit name, which must be lower-case letters.</param>
        /// <param name="ulimit">The limit definition.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithUlimit(string name, UlimitDefinition ulimit)
        {
            Guard.UlimitName(name, nameof(name));
            Guard.NotNull(ulimit, nameof(ulimit));
            _definition = _definition with { Ulimits = Collections.With(_definition.Ulimits, name, ulimit) };
            return this;
        }

        /// <summary>
        /// Sets an extension field on this build. Setting the same key again replaces its value. Compose ignores
        /// these; they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        /// <param name="key">The field key, which must start with <c>x-</c>.</param>
        /// <param name="value">The field value, serialised as-is.</param>
        /// <returns>This builder.</returns>
        public BuildBuilder WithExtension(string key, object? value)
        {
            _definition = _definition with { Extensions = ExtensionsMutator.Set(_definition.Extensions, key, value, nameof(key)) };
            return this;
        }

        /// <summary>
        /// Creates the build definition from the values set so far.
        /// </summary>
        /// <returns>An immutable <see cref="BuildDefinition"/>.</returns>
        public BuildDefinition Build()
        {
            return _definition;
        }
    }
}
