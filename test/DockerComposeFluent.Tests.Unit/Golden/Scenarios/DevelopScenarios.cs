using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>develop</c> golden scenarios.
    /// </summary>
    internal static class DevelopScenarios
    {
        /// <summary>
        /// Every watch action and the settings that go with each, using the shortcut and builder overloads.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("frontend", service => service
                    .WithImage("example/webapp")
                    .WithBuild("./webapp")
                    .WithDevelop(develop => develop
                        .WithSyncWatch("./webapp/html", "/var/www")
                        .WithWatch("./webapp/src", WatchAction.Rebuild)
                        .WithWatch("./webapp/package.json", WatchAction.Restart)
                        .WithWatch(rule => rule
                            .WithPath("./webapp/static")
                            .WithAction(WatchAction.SyncAndRestart)
                            .WithTarget("/var/www/static")
                            .WithIgnore(new[] { "node_modules/", "*.tmp" })
                            .WithInclude("*.css")
                            .WithInitialSync(true))
                        .WithWatch(rule => rule
                            .WithPath("./etc/config")
                            .WithAction(WatchAction.SyncAndExec)
                            .WithTarget("/etc/config/")
                            .WithExec(exec => exec
                                .WithCommand("app reload")
                                .WithUser("root")
                                .WithEnvironment("MODE", "reload"))
                            .WithExtension("x-note", "no downtime"))
                        .WithExtension("x-owner", "frontend team")))
                .Build();
        }
    }
}
