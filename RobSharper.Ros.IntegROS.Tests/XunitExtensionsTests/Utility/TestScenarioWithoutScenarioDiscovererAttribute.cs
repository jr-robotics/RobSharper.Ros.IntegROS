namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class TestScenarioWithoutScenarioDiscovererAttribute : ScenarioAttribute
    {
        public override int GetScenarioHashCode()
        {
            return GetHashCode();
        }
    }
}