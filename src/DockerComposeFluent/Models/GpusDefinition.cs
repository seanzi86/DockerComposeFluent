using System;
using System.Collections.Generic;

namespace DockerComposeFluent.Models
{
    /// <summary>
    /// The GPUs a service may use (the <c>gpus</c> entry): either all of them, written as the string
    /// <c>all</c>, or a list of specific requests.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/05-services.md#gpus"/>
    /// </summary>
    public sealed record GpusDefinition
    {
        /// <summary>
        /// Whether every available GPU is used.
        /// </summary>
        public bool All { get; init; }

        /// <summary>
        /// The specific GPU requests. Empty when <see cref="All"/> is set.
        /// </summary>
        public IReadOnlyList<GpuDefinition> Devices { get; init; } = Array.Empty<GpuDefinition>();
    }
}
