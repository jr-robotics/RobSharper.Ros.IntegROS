using System;
using RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility
{
    [ScenarioDiscoverer("RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility.TestScenarioDiscoverer", "RobSharper.Ros.IntegROS.Tests")]
    public class TestScenarioAttribute : ScenarioAttribute
    {
        private readonly string _key;

        public string Key => _key;

        public TestScenarioDiscoveryBehavior DiscoveryBehavior { get; set; } = TestScenarioDiscoveryBehavior.Default;


        public TestScenarioAttribute() : this(null)
        {
        }

        public TestScenarioAttribute(string key)
        {
            if (string.IsNullOrEmpty(key))
                _key = Guid.NewGuid().ToString("D");
            
            _key = key;
        }
        public override int GetScenarioHashCode()
        {
            return HashCode.Combine(_key);
        }
    }
}