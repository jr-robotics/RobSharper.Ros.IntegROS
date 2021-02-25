using RobSharper.Ros.IntegROS.Scenarios;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery
{
    public interface IScenarioDiscoverer
    {
        IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute);
        
        IScenario GetScenario(IScenarioIdentifier scenarioIdentifier);
    }
}