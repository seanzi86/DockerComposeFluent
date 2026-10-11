namespace DockerComposeFluent.Validation
{
    /// <summary>
    /// The stable codes of the checks run by <c>DockerComposeFile.Validate</c>, for use with
    /// <see cref="ValidationResult.Ignoring(string[])"/> and for filtering
    /// <see cref="ValidationResult.Problems"/>.
    /// </summary>
    public static class ValidationCodes
    {
        /// <summary>
        /// <c>DCF001</c>: a service refers to a service that is not defined, through <c>depends_on</c>,
        /// <c>links</c>, <c>volumes_from</c>, <c>extends</c>, or a <c>service:</c> reference in
        /// <c>network_mode</c>, <c>ipc</c> or <c>pid</c>.
        /// </summary>
        public const string UndefinedService = "DCF001";

        /// <summary>
        /// <c>DCF002</c>: a service joins a network that is not defined in the top-level <c>networks</c>.
        /// </summary>
        public const string UndefinedNetwork = "DCF002";

        /// <summary>
        /// <c>DCF003</c>: a service mounts a named volume that is not defined in the top-level <c>volumes</c>.
        /// </summary>
        public const string UndefinedVolume = "DCF003";

        /// <summary>
        /// <c>DCF004</c>: a service (or its build) uses a secret that is not defined in the top-level
        /// <c>secrets</c>.
        /// </summary>
        public const string UndefinedSecret = "DCF004";

        /// <summary>
        /// <c>DCF005</c>: a service uses a config that is not defined in the top-level <c>configs</c>.
        /// </summary>
        public const string UndefinedConfig = "DCF005";

        /// <summary>
        /// <c>DCF006</c>: services depend on, or extend, each other in a cycle.
        /// </summary>
        public const string DependencyCycle = "DCF006";

        /// <summary>
        /// <c>DCF007</c>: a <c>service_healthy</c> dependency points at a service with no healthcheck, so the
        /// condition can never be met.
        /// </summary>
        public const string MissingHealthcheck = "DCF007";

        /// <summary>
        /// <c>DCF008</c>: two published ports bind the same host port, protocol and address.
        /// </summary>
        public const string DuplicatePublishedPort = "DCF008";
    }
}
