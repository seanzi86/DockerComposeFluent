using System.Collections.Generic;
using DockerComposeFluent.Builders;
using DockerComposeFluent.Models;

namespace DockerComposeFluent.Tests.Unit.Golden.Scenarios
{
    /// <summary>
    /// The <c>environment</c> golden scenarios.
    /// </summary>
    internal static class EnvironmentScenarios
    {
        [Scenario]
        internal static DockerComposeFile Forms()
        {
            return new DockerComposeBuilder()
                .WithService("map", service => service
                    .WithImage("nginx")
                    .WithEnvironment("RACK_ENV", "development")
                    .WithEnvironment("SHOW", true)
                    .WithEnvironment("PORT", 8080)
                    .WithEnvironment("EMPTY", "")
                    .WithEnvironment("USER_INPUT"))
                .WithService("list", service => service
                    .WithImage("nginx")
                    .WithEnvironment(new[] { "RACK_ENV=development", "SHOW=true", "URL=http://x?a=b", "EMPTY=", "USER_INPUT" }))
                .WithService("mixed", service => service
                    .WithImage("nginx")
                    .WithEnvironment("A", "1")
                    .WithEnvironment("B")
                    .WithEnvironment(new[] { "C=3" }))
                .Build();
        }

        /// <summary>
        /// Every way of writing environment variables in one file, including values that YAML would otherwise
        /// read as something other than a string.
        /// </summary>
        [Scenario]
        internal static DockerComposeFile KitchenSink()
        {
            return new DockerComposeBuilder()
                .WithService("typed", service => service
                    .WithImage("nginx")
                    .WithEnvironment("TEXT", "hello world")
                    .WithEnvironment("FLAG", true)
                    .WithEnvironment("OFF", false)
                    .WithEnvironment("COUNT", 8080)
                    .WithEnvironment("BIG", 9007199254740993L)
                    .WithEnvironment("RATIO", 0.5)
                    .WithEnvironment("EMPTY", "")
                    .WithEnvironment("FROM_HOST"))
                .WithService("lookalikes", service => service
                    .WithImage("nginx")
                    .WithEnvironment("YES", "yes")
                    .WithEnvironment("NULL", "null")
                    .WithEnvironment("TILDE", "~")
                    .WithEnvironment("SCIENTIFIC", "1e3")
                    .WithEnvironment("DATE", "2001-12-14")
                    .WithEnvironment("HEX", "0x10")
                    .WithEnvironment("COLON", "a: b")
                    .WithEnvironment("HASH", "a # b")
                    .WithEnvironment("QUOTES", "say \"hi\"")
                    .WithEnvironment("NEWLINE", "line1\nline2"))
                .WithService("dictionary", service => service
                    .WithImage("nginx")
                    .WithEnvironment(new Dictionary<string, string> { ["RACK_ENV"] = "development", ["SHOW"] = "true" }))
                .WithService("entries", service => service
                    .WithImage("nginx")
                    .WithEnvironment(new[] { "RACK_ENV=development", "URL=http://example.com?a=b", "EMPTY=", "FROM_HOST" }))
                .WithService("combined", service => service
                    .WithImage("nginx")
                    .WithEnvironment("A", "1")
                    .WithEnvironment("B")
                    .WithEnvironment(new[] { "C=3" })
                    .WithEnvironment(new Dictionary<string, string> { ["D"] = "4" }))
                .Build();
        }
    }
}
