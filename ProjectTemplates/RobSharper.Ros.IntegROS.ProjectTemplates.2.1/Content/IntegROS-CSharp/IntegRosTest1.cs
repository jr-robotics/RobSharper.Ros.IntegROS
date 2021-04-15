using System;
using System.Linq;
using FluentAssertions;
using RobSharper.Ros.IntegROS;

namespace Company.TestProject1
{
    [RosbagScenario(BagFiles.MyRecordedBag)]
    public class IntegRosTest1 : ForScenario
    {
        [ExpectThat]
        public void Test_case()
        {
            Scenario
                .Messages
                // Add your query here
                .Should()
                // Add your expectation here
                .NotBeEmpty();
        }
        
        [ExpectThat]
        public void Turtle_always_moves_forwards()
        {
            var messages = Scenario
                .Messages
                .InTopic("/turtle*/pose")
                .WithMessageType<Messages.Pose>()
                .Select(message => message.Value.X)
                .Should()
                .BeInAscendingOrder();
        }
    }
}
