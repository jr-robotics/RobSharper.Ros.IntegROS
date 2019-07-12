using System;
using Messages.geometry_msgs;
using RosComponentTesting;
using Xunit;
using Pose = Messages.turtlesim.Pose;

namespace TurtleSimTests
{
    public class HoldPositionTests
    {
        [Fact]
        public void TurtleIsNotMovingWithoutCommand()
        {
            new RosTestBuilder()
                .Expect<Twist>(x => x
                    .Topic("/turtle1/cmd_vel")
                    .Match(It.IsAny<Twist>())
                    .Occurrences(Times.Never)
                )
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.IsAny<Pose>())
                    .Occurrences(Times.AtLeast(2))
                )
                .Execute();
        }
    }
}