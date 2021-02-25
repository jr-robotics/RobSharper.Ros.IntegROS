using IntegROS.Scenarios;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public interface IScenarioDiscoverer
    {
        IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute);
        
        IScenario GetScenario(IScenarioIdentifier scenarioIdentifier);
    }
}