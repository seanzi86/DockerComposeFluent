using System;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Serialisation
{
    /// <summary>
    /// The compose-spec spelling of each enum value.
    /// </summary>
    internal static class EnumNames
    {
        /// <summary>
        /// Gets the spelling of a port protocol, or <c>null</c> when it is not set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compose-spec spelling, or <c>null</c>.</returns>
        internal static string? Of(PortProtocol? value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value.Value)
            {
                case PortProtocol.Tcp:
                    return "tcp";
                case PortProtocol.Udp:
                    return "udp";
                default:
                    throw Unknown(value.Value);
            }
        }

        /// <summary>
        /// Gets the spelling of a port mode, or <c>null</c> when it is not set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compose-spec spelling, or <c>null</c>.</returns>
        internal static string? Of(PortMode? value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value.Value)
            {
                case PortMode.Host:
                    return "host";
                case PortMode.Ingress:
                    return "ingress";
                default:
                    throw Unknown(value.Value);
            }
        }

        /// <summary>
        /// Gets the spelling of a mount type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compose-spec spelling.</returns>
        internal static string Of(MountType value)
        {
            switch (value)
            {
                case MountType.Volume:
                    return "volume";
                case MountType.Bind:
                    return "bind";
                case MountType.Tmpfs:
                    return "tmpfs";
                case MountType.Image:
                    return "image";
                case MountType.Npipe:
                    return "npipe";
                case MountType.Cluster:
                    return "cluster";
                default:
                    throw Unknown(value);
            }
        }

        /// <summary>
        /// Gets the spelling of a bind propagation mode, or <c>null</c> when it is not set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compose-spec spelling, or <c>null</c>.</returns>
        internal static string? Of(BindPropagation? value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value.Value)
            {
                case BindPropagation.Private:
                    return "private";
                case BindPropagation.RPrivate:
                    return "rprivate";
                case BindPropagation.Shared:
                    return "shared";
                case BindPropagation.RShared:
                    return "rshared";
                case BindPropagation.Slave:
                    return "slave";
                case BindPropagation.RSlave:
                    return "rslave";
                default:
                    throw Unknown(value.Value);
            }
        }

        /// <summary>
        /// Gets the spelling of a SELinux label, or <c>null</c> when it is not set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compose-spec spelling, or <c>null</c>.</returns>
        internal static string? Of(SelinuxLabel? value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value.Value)
            {
                case SelinuxLabel.Shared:
                    return "z";
                case SelinuxLabel.Private:
                    return "Z";
                default:
                    throw Unknown(value.Value);
            }
        }

        /// <summary>
        /// Gets the spelling of a restart policy.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compose-spec spelling.</returns>
        internal static string Of(RestartPolicy value)
        {
            switch (value)
            {
                case RestartPolicy.No:
                    return "no";
                case RestartPolicy.Always:
                    return "always";
                case RestartPolicy.OnFailure:
                    return "on-failure";
                case RestartPolicy.UnlessStopped:
                    return "unless-stopped";
                default:
                    throw Unknown(value);
            }
        }

        private static ArgumentOutOfRangeException Unknown<T>(T value)
            where T : struct
        {
            return new ArgumentOutOfRangeException(nameof(value), value, "Unknown " + typeof(T).Name + " value.");
        }
    }
}
