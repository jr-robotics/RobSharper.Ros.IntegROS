using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTest : XunitTest
    {
        public IScenarioIdentifier ScenarioIdentifier { get; }

        public ScenarioTest(ScenarioTestCase testCase, string displayName) 
            : base(testCase, displayName)
        {
            ScenarioIdentifier = testCase.ScenarioIdentifier;
        }

        public ScenarioTest(MultipleScenariosTestCase testCase, IScenarioIdentifier scenarioIdentifier, string displayName) 
            : base(testCase, displayName)
        {
            ScenarioIdentifier = scenarioIdentifier;
        }
        
        protected ScenarioTest(IXunitTestCase testCase, IScenarioIdentifier scenarioIdentifier, string displayName) 
            : base(testCase, displayName)
        {
        }
    }
}