using System;

namespace DockerComposeFluent.Tests.Unit.Golden
{
    /// <summary>
    /// Marks a static parameterless method returning a <c>DockerComposeFile</c> as a golden scenario. The feature
    /// comes from the containing class (<c>PortsScenarios</c> is <c>ports</c>) and the scenario name from the
    /// method (<c>QuickStart</c> is <c>quick-start</c>), so its fixture is
    /// <c>Golden/Fixtures/&lt;feature&gt;/&lt;name&gt;.verified.yml</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class ScenarioAttribute : Attribute
    {
    }
}
