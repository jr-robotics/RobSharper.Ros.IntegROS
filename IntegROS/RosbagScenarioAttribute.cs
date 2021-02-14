using System;
using IntegROS.XunitExtensions.ScenarioDiscovery;

namespace IntegROS
{
    [ScenarioDiscoverer("IntegROS.XunitExtensions.ScenarioDiscovery.RosbagScenarioDiscoverer", "IntegROS")]
    public class RosbagScenarioAttribute : ScenarioAttribute
    {
        public string Bagfile { get; }

        public RosbagScenarioAttribute(string bagfile)
        {
            Bagfile = bagfile;
        }

        public override int GetScenarioHashCode()
        {
            return HashCode.Combine(Bagfile);
        }
    }
}