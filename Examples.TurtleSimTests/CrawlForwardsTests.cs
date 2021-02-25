using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using RobSharper.Ros.IntegROS;
using RobSharper.Ros.MessageEssentials;

namespace Examples.TurtleSimTests
{
    [RosbagScenario(TurtleSimBagFiles.MoveForwards, DisplayName = nameof(TurtleSimBagFiles.MoveForwards))]
    public class CrawlForwardsTests : ForScenario
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

        [ExpectThat]
        public void Turtle_always_moves_forwards()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .WithMessageType<Pose>()
                .Select(message => message.Value.X)
                .Should()
                .BeInAscendingOrder();
        }

        [ExpectThat]
        public void Turtle_always_moves_forwards_grouped()
        {
            var poses = Scenario.Messages
                .InTopic("/turtle*/pose")
                .WithMessageType<Pose>()
                .GroupBy(t => t.Topic);
            
            foreach (var turtle in poses)
            {
                turtle
                    .Select(m => m.Value.X)
                    .Should()
                    .BeInAscendingOrder();
            }
        }

        [ExpectThat]
        public void Turtle_does_not_move_sidewards__variant_1()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .SelectMessages<Pose>();

            var first = messages.First();

            foreach (var pose in messages)
            {
                pose.Y.Should().Be(first.Y);
            }
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
                .HaveCount(1);
        }
        
        [ExpectThat]
        public void Turtle_does_not_move_sidewards__variant_3()
        {
            Scenario.Messages
                .InTopic("/turtle*/pose")
                .WithMessageType<Pose>()
                .Select(message => message.Value.Y)
                .Should()
                .AllBeTheSame();
        }

        [ExpectThat]
        public void Turtle_does_not_turn()
        {
            var messages = Scenario.Messages
                .InTopic("/turtle*/pose")
                .SelectMessages<Pose>();

            messages
                .Select(pose => pose.Theta)
                .Should()
                .AllBeTheSame($"{nameof(Pose.Theta)} should not change");

            messages
                .Select(pose => pose.AngularVelocity)
                .Should()
                .AllBeEquivalentTo(0f, $"{nameof(Pose.AngularVelocity)} should be 0");
        }
    }

    
    public static class TestAssertionExtensions
    {
        [CustomAssertion]
        public static AndConstraint<GenericCollectionAssertions<T>> AllBeTheSame<T>(this GenericCollectionAssertions<T> target,
            string because = "",
            params object[] args)
        {
            if (target.Subject == null || !target.Subject.Any())
                return new AndConstraint<GenericCollectionAssertions<T>>(target);
            
            var expectedValue = target.Subject.FirstOrDefault();

            foreach (var actualValue in target.Subject)
            {
                Execute.Assertion
                    .ForCondition(object.Equals(expectedValue, actualValue))
                    .BecauseOf(because, args)
                    .FailWith("Expected all values of {context:collection} to be {0}{reason}, but found {1}.", expectedValue, actualValue);
            }
            
            return new AndConstraint<GenericCollectionAssertions<T>>(target);
        }
    }
}