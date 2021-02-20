using System;
using FluentAssertions;
using RosComponentTesting;
using RosComponentTesting.RosNet;
using Uml.Robotics.Ros.Messages.geometry_msgs;
using Xunit;
using Xunit.Abstractions;
using Pose = Messages.turtlesim.Pose;

namespace TurtleSimTests
{
    public class HoldPositionTests
    {
        private readonly ITestOutputHelper _output;

        public HoldPositionTests(ITestOutputHelper output)
        {
            this._output = output;
        }
        
        [Fact]
        public void TurtleIsNotMovingWithoutCommand()
        {
            new TestBuilder()
                .UseRosDotNet()
                .Wait(TimeSpan.FromSeconds(5))
                .Expect<Twist>(x => x
                    .Topic("/turtle1/cmd_vel")
                    .Match(It.IsAny<Twist>())
                    .Occurrences(Times.Never)
                )
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.IsAny<Pose>())
                )
                .Execute();
        }
        
        [Fact(Skip = "No failing tests")]
        public void Failing_TurtleIsNotMovingWithoutCommand()
        {
            new TestBuilder()
                .UseRosDotNet()
                .Wait(TimeSpan.FromSeconds(5))
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
        public void When_not_moving_velocity_is_zero_example_a()
        {
            new TestBuilder()
                .UseRosDotNet()
                .Wait(TimeSpan.FromSeconds(3))
                .Expect<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.IsAny<Pose>())
                    .Occurrences(Times.AtLeast(1))
                    .Callback(p =>
                    {
                        p.angular_velocity.Should().Be(0, "because turtle is not moving");
                        p.linear_velocity.Should().Be(0, "because turtle is not moving");
                    })
                )
                .Execute();
        }
        
        [Fact]
        public void When_not_moving_velocity_is_zero_example_b()
        {
            const int expectedOccurrences = 25;
            
            int cnt = 0;
            new TestBuilder()
                .UseRosDotNet()
                .WaitFor<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.IsAny<Pose>())
                    .Occurrences(Times.Exactly(expectedOccurrences))
                    .Callback(p =>
                    {
                        cnt++;
                        p.angular_velocity.Should().Be(0, "because turtle is not moving");
                        p.linear_velocity.Should().Be(0, "because turtle is not moving");
                    })
                    .Timeout(TimeSpan.FromSeconds(3))
                )
                .Execute();

            cnt.Should().Be(expectedOccurrences, "because we wanted to wait for specific number of occurrences");
        }

        [Fact]
        public void Publish_test()
        {
            var move1 = new Twist
            {
                linear =
                {
                    x = 1,
                }
            };
            var move2 = new Twist
            {
                linear =
                {
                    x = -1,
                }
            };

            new TestBuilder()
                .UseRosDotNet()
                .WaitFor<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.Matches<Pose>(m => m.x > 5))
                    .Timeout(TimeSpan.FromSeconds(3))
                )
                .Publish("/turtle1/cmd_vel", move1)
                .WaitFor<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.Matches<Pose>(m => m.x > 6.55244))
                )
                .Publish("/turtle1/cmd_vel", move2)
                .Expect<Twist>(x => x
                    .Topic("/turtle1/cmd_vel")
                    .Match(It.IsAny<Twist>())
                    .Occurrences(Times.Exactly(2))
                )
                .WaitFor<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.Matches<Pose>(m => m.x > 5))
                    .Timeout(TimeSpan.FromSeconds(3))
                )
                .Execute();
        }
    }
}