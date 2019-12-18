using System;
using Messages.geometry_msgs;
using RosComponentTesting;
using RosComponentTesting.RosNet;
using Xunit;
using Pose = Messages.turtlesim.Pose;

namespace TurtleSimTests
{
    public class KeyboardInteractionTest
    {
        [Fact]
        public void BehaviourTest()
        {
            int callCount = 0;
            Pose startPose = null;
            
            new TestBuilder()
                .UseRosDotNet()
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    //.Match(It.IsAny<Pose>())
                    //.Occurrences(Times.Exactly(1))
                    .Callback(p =>
                    {
                        callCount++;
                        startPose = p;
                    })
                )
                .WaitFor<Twist>(x => x
                    .Topic("/turtle1/cmd_vel")
                    .Match(It.IsAny<Twist>())
                    //.Timeout(TimeSpan.FromSeconds(15))
                )
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(pose => pose.linear_velocity > 0)
                    .Occurrences(Times.Exactly(14))
                    .Timeout(TimeSpan.FromSeconds(4))
                )
                .Execute();
        }
    }
}