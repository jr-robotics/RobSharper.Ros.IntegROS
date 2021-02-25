using RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility
{
    [ScenarioDiscoverer("RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility.NotExistingTestScenarioDiscoverer", "RobSharper.Ros.IntegROS.Tests")]
    public class TestScenarioWithInvalidScenarioDiscovererAttribute : ScenarioAttribute
    {
        public override int GetScenarioHashCode()
        {
            return GetHashCode();
        }
    }
}