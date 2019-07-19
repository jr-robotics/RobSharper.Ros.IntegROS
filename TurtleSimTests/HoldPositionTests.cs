using System;
using Messages.geometry_msgs;
using RosComponentTesting;
using Uml.Robotics.Ros;
using Xunit;
using Xunit.Abstractions;
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

        [Fact (Skip = "Runs infinite")]
        public void Test1()
        {
            ROS.ROS_MASTER_URI = "http://localhost:11311";
            
            ROS.ROS_MASTER_URI = "http://localhost:11311";
            ROS.Init(new string[0], "TESTNODE");

            var spinner = new AsyncSpinner();
            spinner.Start();

            var node = new NodeHandle();
            node.Subscribe<Pose>("/turtle1/pose", 1, p => output.WriteLine(p.MessageType));
            
            ROS.WaitForShutdown();
        }
    }
}