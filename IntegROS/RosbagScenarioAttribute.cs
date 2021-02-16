using System;
using IntegROS.XunitExtensions.ScenarioDiscovery;

namespace IntegROS
{
    /// <summary>
    /// Scenario attribute for a recorded ROSBAG file.
    /// </summary>
    [ScenarioDiscoverer("IntegROS.XunitExtensions.ScenarioDiscovery.RosbagScenarioDiscoverer", "IntegROS")]
    public class RosbagScenarioAttribute : ScenarioAttribute
    {
        /// <summary>
        /// The path to the ROSBAG file
        /// </summary>
        public string Bagfile { get; }

        public RosbagScenarioAttribute(string bagfile)
        {
            Bagfile = bagfile;
        }

        /// <inheritdoc cref="ScenarioAttribute"/>
        public override int GetScenarioHashCode()
        {
            return HashCode.Combine(Bagfile);
        }
    }
}