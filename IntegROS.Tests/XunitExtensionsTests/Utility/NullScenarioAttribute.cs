using System;
using IntegROS.XunitExtensions.ScenarioDiscovery;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    [ScenarioDiscoverer("IntegROS.Tests.XunitExtensionsTests.Utility.NullScenarioDiscoverer", "IntegROS.Tests")]
    public class NullScenarioAttribute : ScenarioAttribute
    {
        public static readonly  Guid NullId = new Guid("535124A8-20E9-4706-877F-4C99F9513454");

        public NullScenarioAttribute()
        {
        }

        public override int GetScenarioHashCode()
        {
            return HashCode.Combine(NullId);
        }
    }
}