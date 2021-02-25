using System;
using FluentAssertions;
using Messages.geometry_msgs;
using Messages.turtlesim;
using RosComponentTesting;
using RosComponentTesting.RosNet;
using Uml.Robotics.Ros.Messages.geometry_msgs;
using Xunit;
using Pose = Messages.turtlesim.Pose;

namespace TurtleSimTests
{
    public static class ExampleTestBuilderExtensions
    {
        public static TestBuilder CallService<TRequestType>(this TestBuilder builder, string service, TRequestType request)
        {
            return builder;
        }

        public static bool IsApproximately(this float value, float expected, float tolerance)
        {
            return Math.Abs(value - expected) < tolerance;
        }
    }
    
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
        
        
        
        [Fact]
        public void CrawlForward()
        {
            new TestBuilder()
                .CallService("/turtle1/teleport_absolute", new TeleportAbsolute.Request { x = 5, y = 5, theta = 0 })
                .Publish("/turtle1/cmd_vel", new Twist
                    {
                        linear = new Vector3 { x = 2, y = 0, z = 0 },
                        angular = new Vector3 { x = 0, y = 0, z = 0 }
                    })
                .Expect<Pose>(x => x
                    .Name("crawl")
                    .Topic("/turtle1/pose")
                    .Match(pose => pose.linear_velocity == 2)
                    .Callback(pose =>
                    {
                        pose.angular_velocity.Should().Be(0);
                    })
                    .Timeout(TimeSpan.FromMilliseconds(250))
                )
                .Expect<Pose>(x => x
                    .Name("stop")
                    .DependsOn("crawl")
                    .Topic("/turtle1/pose")
                    .Match(pose => pose.linear_velocity == 0)
                    .Callback(pose =>
                    {
                        pose.x.Should().BeInRange(7.00f, 7.02f);
                        pose.y.Should().Be(5);
                        pose.theta.Should().Be(0);
                    })
                    .Timeout(TimeSpan.FromSeconds(2))
                )
                .Execute();
        }

        [Theory]
        [InlineData(2.0)]
        [InlineData(-2.0)]
        [InlineData(3.5)]
        [InlineData(-3.5)]
        public void Crawl(float linearVelocity)
        {
            new TestBuilder()
                .CallService("/turtle1/teleport_absolute", new TeleportAbsolute.Request { x = 5, y = 5, theta = 0 })
                .Publish("/turtle1/cmd_vel", new Twist
                {
                    linear = new Vector3 { x = linearVelocity, y = 0, z = 0 },
                    angular = new Vector3 { x = 0, y = 0, z = 0 }
                })
                .Expect<Pose>(x => x
                    .Name("crawl")
                    .Topic("/turtle1/pose")
                    .Match(pose => pose.linear_velocity == linearVelocity)
                    .Callback(pose =>
                    {
                        pose.angular_velocity.Should().Be(0);
                    })
                    .Timeout(TimeSpan.FromMilliseconds(250))
                )
                .Expect<Pose>(x => x
                    .Name("stop")
                    .DependsOn("crawl")
                    .Topic("/turtle1/pose")
                    .Match(pose => pose.linear_velocity == 0)
                    .Callback(pose =>
                    {
                        pose.x.Should().BeApproximately(5f + linearVelocity, 0.02f);
                        pose.y.Should().Be(5);
                        pose.theta.Should().Be(0);
                    })
                    .Timeout(TimeSpan.FromSeconds(2))
                )
                .Execute();
        }
    }
}