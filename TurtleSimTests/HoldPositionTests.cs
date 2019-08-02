using System;
using System.Diagnostics;
using System.Threading;
using FluentAssertions;
using Messages.geometry_msgs;
using RosComponentTesting;
using Uml.Robotics.Ros;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using Int32 = Messages.std_msgs.Int32;
using Pose = Messages.turtlesim.Pose;

namespace TurtleSimTests
{
    public class HoldPositionTests
    {
        private readonly ITestOutputHelper output;

        public HoldPositionTests(ITestOutputHelper output)
        {
            this.output = output;
            

            ROS.ROS_MASTER_URI = "http://localhost:11311";
            ROS.Init(new string[0], "TESTNODE");
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

            new RosTestBuilder()
                .WaitFor<Pose>(x => x
                    .Topic("/turtle1/pose")
                    .Match(It.Matches<Pose>(m => m.x > 5))
                    .Timeout(TimeSpan.FromSeconds(3))
                )
//                .Wait(TimeSpan.FromSeconds(8))
//                .Publish("/turtle1/cmd_vel", move1)
//                .WaitFor<Pose>(x => x
//                    .Topic("/turtle1/pose")
//                    .Match(It.Matches<Pose>(m => m.x < 4.555))
//                )
//                .Wait(TimeSpan.FromSeconds(5))
//                .Publish("/turtle1/cmd_vel", move2)
//                .Expect<Twist>(x => x
//                    .Topic("/turtle1/cmd_vel")
//                    .Match(It.IsAny<Twist>())
//                    .Occurrences(Times.Exactly(2))
//                )
                .Execute();
        }

        [Fact]
        public void PublishTest()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            ROS.ROS_MASTER_URI = "http://localhost:11311";
            ROS.Init(new string[0], "TESTNODE");
            NodeHandle node = new NodeHandle();
            
            var intPublisher = node.Advertise<Messages.std_msgs.Int32>("/x/int/", 1);
            
            var intMsg = new Int32();

            while (ROS.OK)
            {
                Thread.Sleep(1000);
                intMsg.data++;
                
                intPublisher.Publish(intMsg);
            }
            
            ROS.Shutdown();
        }

        [Fact]
        public void PublishTwistTest()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            ROS.ROS_MASTER_URI = "http://localhost:11311";
            ROS.Init(new string[0], "TESTNODE");
            
            var spinner = new SingleThreadSpinner();
            NodeHandle node = new NodeHandle();

            var intPublisher = node.Advertise<Twist>("/turtle1/cmd_vel", 
                1, 
                publisher => { this.output.WriteLine("TODO"); },
                publisher => { this.output.WriteLine("TODO"); });

            var msg = new Twist();

            while (ROS.OK)
            {
                Thread.Sleep(1000);
                msg.linear.x += 1.0;
                
                intPublisher.Publish(msg);
            }
            
            ROS.Shutdown();
        }

        [Fact]
        public void Test2()
        {
            Assert.InRange(15, 3, 10);
            Assert.True(false, "My User Message");
        }
    }
}