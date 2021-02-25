using System;
using RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery;

namespace RobSharper.Ros.IntegROS
{
    /// <summary>
    /// Scenario attribute for a recorded ROSBAG file.
    /// </summary>
    [ScenarioDiscoverer("RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery.RosbagScenarioDiscoverer", "RobSharper.Ros.IntegROS")]
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