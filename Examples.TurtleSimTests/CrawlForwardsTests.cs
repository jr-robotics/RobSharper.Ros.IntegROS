using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Types;
using IntegROS;
using RobSharper.Ros.MessageEssentials;

namespace Examples.TurtleSimTests
{
    public class CrawlForwardsTests : ForScenario<RosbagScenario>
    {
        [RosMessage("turtlesim/Pose")]
        public class Pose
        {
            /*
                float32 x
                float32 y
                float32 theta

                float32 linear_velocity
                float32 angular_velocity
            */
            
            [RosMessageField(1, "float32", "x" )]
            public float X { get; set; }
            
            [RosMessageField(2, "float32", "y" )]
            public float Y { get; set; }
            
            [RosMessageField(3, "float32", "theta" )]
            public float Theta { get; set; }
            
            [RosMessageField(4, "float32", "linear_velocity" )]
            public float LinearVelocity { get; set; }
            
            [RosMessageField(5, "float32", "angular_velocity" )]
            public float AngularVelocity { get; set; }
        }
        
        public CrawlForwardsTests(RosbagScenario scenario) : base(scenario)
        {
            scenario.Load(TurtleSimBagFiles.MoveForwards);
        }

        [ExpectThat]
        public void Turtle_always_moves_forwards()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .SelectMessages<Pose>();
            
            messages
                .Should()
                .BeInAscendingOrder(pose => pose.X);
        }

        [ExpectThat]
        public void Turtle_does_not_move_sidewards__variant1()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .SelectMessages<Pose>();

            var first = messages.First();

            foreach (var pose in messages)
            {
                pose.Y.Should().Be(first.Y);
            }
            
            // messages
            //     .Should()
            //     .AllBeEquivalentTo();
            
            // //TODO besser noch:
            // messages
            //     .Should()
            //     .AllBeEquivalent(pose => pose.Y);
        }
        
        [ExpectThat]
        public void Turtle_does_not_move_sidewards__variant_2()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .SelectMessages<Pose>();
            
            messages
                .Select(pose => pose.Y)
                .Distinct()
                .Should()
                .HaveCount(2);
        }

        [ExpectThat]
        public void Turtle_does_not_turn()
        {
            throw new NotImplementedException();
        }
        
        
    }
}