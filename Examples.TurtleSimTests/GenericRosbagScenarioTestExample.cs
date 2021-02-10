// using System.Linq;
// using FluentAssertions;
// using IntegROS;
// using IntegROS.Rosbag;
// using IntegROS.Scenarios;
// using Moq;
//
// namespace Examples.TurtleSimTests
// {
//     public class GenericRosbagScenarioTestExample : ForScenario<RosbagScenario>
//     {
//         public GenericRosbagScenarioTestExample(RosbagScenario scenario) : base(scenario)
//         {
//             ShouldContainMessages = false;
//             
//             //TODO: Replace stub when rosbag reader is available
//             RosbagReader.Instance = new Mock<IRosbagReader>().Object;
//             //scenario.Load("rosbagfilepath.bag");
//         }
//
//         [ExpectThat]
//         public void Publishes_pose()
//         {
//             Scenario.Messages
//                 .Should()
//                 .NotContain(message => message.Topic == "/turtle1/pose", "no pose should be published");
//
//         }
//         
//         [ExpectThat]
//         public void No_pose_is_published()
//         {
//             Scenario.Messages
//                 .Where(m => m.Topic == "/turtle1/pose")
//                 .Count()
//                 .Should().Be(0, "no pose should be published");
//         }
//     }
// }