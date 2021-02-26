namespace RobSharper.Ros.IntegROS.Tests
{
    public class RosbagScenarioAttributeTests : ScenarioAttributeTestsBase
    {
        protected override void CreateIdenticalScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b)
        {
            a = new RosbagScenarioAttribute("bags/rosbag-identical.bag");
            b = new RosbagScenarioAttribute("bags/rosbag-identical.bag");
        }

        protected override void CreateDifferentScenarioAttributes(out ScenarioAttribute a, out ScenarioAttribute b)
        {
            a = new RosbagScenarioAttribute("bags/rosbag-a.bag");
            b = new RosbagScenarioAttribute("bags/rosbag-b.bag");
        }
    }
}