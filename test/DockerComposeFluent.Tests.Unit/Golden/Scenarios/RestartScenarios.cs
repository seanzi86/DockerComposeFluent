using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>restart</c> golden scenarios.
    /// </summary>
    internal static class RestartScenarios
    {
        /// <summary>
        /// Every restart policy, and the ways of writing them: the enum, the retry-limited helper and the text form.
        /// <c>no</c> is written quoted, because unquoted YAML reads it as false.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("never", service => service.WithImage("busybox").WithRestart(RestartPolicy.No))
                .WithService("always", service => service.WithImage("busybox").WithRestart(RestartPolicy.Always))
                .WithService("unless-stopped", service => service.WithImage("busybox").WithRestart(RestartPolicy.UnlessStopped))
                .WithService("on-failure", service => service.WithImage("busybox").WithRestart(RestartPolicy.OnFailure))
                .WithService("on-failure-limited", service => service.WithImage("busybox").WithRestartOnFailure(5))
                .WithService("text", service => service.WithImage("busybox").WithRestart("on-failure:3"))
                .WithService("unset", service => service.WithImage("busybox"))
                .Build();
        }
    }
}
