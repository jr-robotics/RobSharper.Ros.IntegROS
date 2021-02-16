using IntegROS.Scenarios;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public class DummyScenarioDiscoverer : IScenarioDiscoverer
    {
        public IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute)
        {
            var displayName = scenarioAttribute != null
                ? ScenarioAttribute.GetAttributeDefinition(scenarioAttribute)
                : "unknown";

            return new DummyScenarioIdentifier(displayName);
        }

        public IScenario GetScenario(IScenarioIdentifier scenarioIdentifier)
        {
            return null;
        }
    }
}