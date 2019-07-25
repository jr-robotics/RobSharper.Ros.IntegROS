using System;
using System.Threading;
using FluentAssertions;
using Messages.geometry_msgs;
using RosComponentTesting;
using Uml.Robotics.Ros;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using Pose = Messages.turtlesim.Pose;

namespace TurtleSimTests
{
    public class HoldPositionTests
    {
        private readonly ITestOutputHelper output;

        public HoldPositionTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        
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
        
        [Fact]
        public void Failing_TurtleIsNotMovingWithoutCommand()
        {
            new RosTestBuilder()
                .Expect<Twist>(x => x
                    .Topic("/turtle1/cmd_vel")
                    .Match(It.IsAny<Twist>())
                    //.Occurrences(Times.Never)
                    .Occurrences(Times.AtLeast(2))
                )
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.IsAny<Pose>())
                    //.Occurrences(Times.AtLeast(2))
                    .Occurrences(Times.Never)
                )
                .Execute();
        }
        
        [Fact]
        public void When_not_moving_velocity_is_zero()
        {
            new RosTestBuilder()
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.IsAny<Pose>())
                    .Occurrences(Times.AtLeast(1))
                    .Callback(p =>
                    {
                        p.angular_velocity.Should().Be(0, "because turtle is not moving");
                        p.linear_velocity.Should().Be(0, "because turtle is not moving");

                        p.linear_velocity.Should().BeGreaterThan(0, "because we need a failing test");
                    })
                )
                .Execute();
        }

        [Fact]
        public void Test2()
        {
            Assert.InRange(15, 3, 10);
            Assert.True(false, "My User Message");
        }
    }
}