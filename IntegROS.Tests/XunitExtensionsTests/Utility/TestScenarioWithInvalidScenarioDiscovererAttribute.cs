using IntegROS.XunitExtensions.ScenarioDiscovery;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    [ScenarioDiscoverer("IntegROS.Tests.XunitExtensionsTests.Utility.NotExistingTestScenarioDiscoverer", "IntegROS.Tests")]
    public class TestScenarioWithInvalidScenarioDiscovererAttribute : ScenarioAttribute
    {
        public override int GetScenarioHashCode()
        {
            return GetHashCode();
        }
    }
}