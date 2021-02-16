using IntegROS.Scenarios;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class NullScenarioDiscoverer : IScenarioDiscoverer
    {
        public IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute)
        {
            return new NullScenarioIdentifier(this.GetType());
        }

        public IScenario GetScenario(IScenarioIdentifier scenarioIdentifier)
        {
            return null;
        }
    }
}