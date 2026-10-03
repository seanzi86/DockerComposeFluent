using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// How to build a service's image from source (the <c>build</c> entry). A build with only a
    /// <see cref="Context"/> is written to YAML as a plain string; any other setting makes it a mapping.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md"/>
    /// </summary>
    public sealed record BuildDefinition
    {
        /// <summary>
        /// The additional named build contexts, keyed by name with the location as the value, as specified by <c>additional_contexts</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#additional_contexts"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> AdditionalContexts { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The build arguments, keyed by name, as specified by <c>args</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#args"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> Args { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The sources to use as the image cache, as specified by <c>cache_from</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#cache_from"/>
        /// </summary>
        public IReadOnlyList<string> CacheFrom { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The locations to export the build cache to, as specified by <c>cache_to</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#cache_to"/>
        /// </summary>
        public IReadOnlyList<string> CacheTo { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The path to the build context: a directory containing a Dockerfile, or a git repository URL, as specified by <c>context</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#context"/>
        /// </summary>
        public string? Context { get; init; }

        /// <summary>
        /// The Dockerfile to build from, relative to the context, as specified by <c>dockerfile</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#dockerfile"/>
        /// </summary>
        public string? Dockerfile { get; init; }

        /// <summary>
        /// The Dockerfile contents, defined inline, as specified by <c>dockerfile_inline</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#dockerfile_inline"/>
        /// </summary>
        public string? DockerfileInline { get; init; }

        /// <summary>
        /// The extra privileged entitlements the build is granted, for example <c>network.host</c>, as specified by <c>entitlements</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#entitlements"/>
        /// </summary>
        /// <remarks>Requires Compose 2.27.0 or later.</remarks>
        public IReadOnlyList<string> Entitlements { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The additional hostnames added to the build containers' <c>/etc/hosts</c>, keyed by hostname with the
        /// address as the value, as specified by <c>extra_hosts</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#extra_hosts"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> ExtraHosts { get; init; } = Collections.EmptyDictionary<string>();

        /// <summary>
        /// The container isolation technology used for the build; supported values are platform-specific, as specified by <c>isolation</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#isolation"/>
        /// </summary>
        public string? Isolation { get; init; }

        /// <summary>
        /// The labels applied to the built image, as specified by <c>labels</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#labels"/>
        /// </summary>
        public Labels Labels { get; init; } = new Labels();

        /// <summary>
        /// The network the build containers use during <c>RUN</c> instructions, as specified by <c>network</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#network"/>
        /// </summary>
        public string? Network { get; init; }

        /// <summary>
        /// Whether to build without using the cache, as specified by <c>no_cache</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#no_cache"/>
        /// </summary>
        public bool? NoCache { get; init; }

        /// <summary>
        /// The stages that are built without the cache, as specified by <c>no_cache_filter</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#no_cache"/>
        /// </summary>
        /// <remarks>Requires Compose 5.0.0 or later.</remarks>
        public IReadOnlyList<string> NoCacheFilter { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The platforms to build the image for, for example <c>linux/amd64</c>, as specified by <c>platforms</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#platforms"/>
        /// </summary>
        public IReadOnlyList<string> Platforms { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Whether the build runs with extended privileges, as specified by <c>privileged</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#privileged"/>
        /// </summary>
        public bool? Privileged { get; init; }

        /// <summary>
        /// The provenance attestation to add to the image: <c>true</c>, <c>false</c> or a setting such as <c>mode=max</c>, as specified by <c>provenance</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#provenance"/>
        /// </summary>
        /// <remarks>Requires Compose 2.39.0 or later.</remarks>
        public string? Provenance { get; init; }

        /// <summary>
        /// Whether to always pull newer versions of the base images, as specified by <c>pull</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#pull"/>
        /// </summary>
        public bool? Pull { get; init; }

        /// <summary>
        /// The SBOM attestation to add to the image: <c>true</c>, <c>false</c> or a setting, as specified by <c>sbom</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#sbom"/>
        /// </summary>
        /// <remarks>Requires Compose 2.39.0 or later.</remarks>
        public string? Sbom { get; init; }

        /// <summary>
        /// The secrets the build can access, as specified by <c>secrets</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#secrets"/>
        /// </summary>
        public IReadOnlyList<SecretReference> Secrets { get; init; } = Array.Empty<SecretReference>();

        /// <summary>
        /// The size of <c>/dev/shm</c> for the build, as a compose-spec byte value such as <c>64m</c>, as specified by <c>shm_size</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#shm_size"/>
        /// </summary>
        public string? ShmSize { get; init; }

        /// <summary>
        /// The SSH agent sockets or keys the build can use, as <c>default</c> or <c>id=path</c>, as specified by <c>ssh</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ssh"/>
        /// </summary>
        public IReadOnlyList<string> Ssh { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The extra tags to apply to the built image, as specified by <c>tags</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#tags"/>
        /// </summary>
        public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();

        /// <summary>
        /// The Dockerfile stage to build, as specified by <c>target</c>.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#target"/>
        /// </summary>
        public string? Target { get; init; }

        /// <summary>
        /// The resource limits for the build containers, keyed by limit name such as <c>nofile</c>, as specified
        /// by <c>ulimits</c>. Empty when none are set.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/build.md#ulimits"/>
        /// </summary>
        public IReadOnlyDictionary<string, UlimitDefinition> Ulimits { get; init; } = Collections.EmptyDictionary<UlimitDefinition>();

        /// <summary>
        /// The extension fields defined on this build, keyed by their <c>x-</c> name. Compose ignores these;
        /// they exist for the file's own reuse (YAML anchors) or for tooling.
        /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md"/>
        /// </summary>
        public IReadOnlyDictionary<string, object?> Extensions { get; init; } = Collections.EmptyDictionary<object?>();

        /// <summary>
        /// Whether only a <see cref="Context"/> is set, so the build is just a context string.
        /// </summary>
        public bool IsContextOnly
        {
            get { return Context != null && (this with { Context = null }).Equals(new BuildDefinition()); }
        }
    }
}
