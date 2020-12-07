using System.Linq;
using FluentAssertions;
using IntegROS;
using Moq;

namespace Examples.TurtleSimTests
{
    public class ScenarioTestExample : ForScenario<RosbagScenario>
    {
        public ScenarioTestExample(RosbagScenario scenario) : base(scenario)
        {
            //TODO: Replace stub when rosbag reader is available
            RosbagReader.Instance = new Mock<IRosbagReader>().Object;
            
            scenario.Load("rosbagfilepath.bag");
        }
        
        [ExpectThat]
        public void No_pose_is_published()
        {
            Scenario.Messages
                .Where(m => m.Topic == "/turtle1/pose")
                .Count()
                .Should().Be(0, "no pose should be published");
        }
    }
}