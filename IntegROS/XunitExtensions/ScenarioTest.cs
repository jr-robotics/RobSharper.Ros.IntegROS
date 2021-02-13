using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTest : XunitTest
    {
        public IScenarioIdentifier ScenarioIdentifier { get; }

        public ScenarioTest(IXunitTestCase testCase, string displayName, IScenarioIdentifier scenarioIdentifier) : base(testCase, displayName)
        {
            ScenarioIdentifier = scenarioIdentifier;
        }
    }
}