using System;
using IntegROS.XunitExtensions.ScenarioDiscovery;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    [ScenarioDiscoverer("IntegROS.Tests.XunitExtensionsTests.Utility.TestScenarioDiscoverer", "IntegROS.Tests")]
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