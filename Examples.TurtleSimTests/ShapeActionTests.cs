using System.Collections.Generic;
using System.Linq;
using Examples.TurtleSimTests.Messages;
using FluentAssertions;
using RobSharper.Ros.IntegROS;
using RobSharper.Ros.IntegROS.Ros.Actionlib;
using RobSharper.Ros.IntegROS.Ros.Messages;

namespace Examples.TurtleSimTests
{
    [RosbagScenario(TurtleSimBagFiles.RectangleShapeAction)]
    [RosbagScenario(TurtleSimBagFiles.TriangleShapeAction)]
    public class ShapeActionTests : ForScenario
    {
        [ExpectThat]
        public void Shape_action_executes_successfully()
        {
            var shapeActions = Scenario.Messages
                .ForActionCalls("/turtle_shape")
                .Select(x => x.FinalState);

            shapeActions.Should().NotBeEmpty();
            shapeActions.Should().AllBeEquivalentTo(GoalStatusValue.Succeeded);
        }
        
        [ExpectThat]
        public void Scenario_has_exactly_one_action_call()
        {
            Scenario.Messages
                .ForActionCalls("/turtle_shape")
                .Should()
                .HaveCount(1);
        }
        
        [ExpectThat]
        public void Scenario_has_only_one_turtle()
        {
            Scenario.Messages
                .InTopic("/turtle*/pose")
                .Select(x => x.Topic)
                .Distinct()
                .Should()
                .HaveCount(1);
        }

        private IEnumerable<IRecordedMessage<Pose>> TurtlePoses => Scenario.Messages
            .InTopic("/turtle*/pose")
            .WithMessageType<Pose>();

        private RosActionCall ActionCall => Scenario.Messages
            .ForActionCalls("/turtle_shape")
            .First();
        
        
        [ExpectThat]
        public void Shape_ends_at_initial_position()
        {
            var initialPose = TurtlePoses
                .LastBefore(ActionCall.GoalMessage)
                .Value;

            var endPose = TurtlePoses
                .FirstAfter(ActionCall.ResultMessage)
                .Value;

            initialPose.X.Should().BeApproximately(endPose.X, 0.0001f);
            initialPose.Y.Should().BeApproximately(endPose.Y, 0.0001f);
        }

        [ExpectThat]
        public void Turtle_does_not_move_after_action_finishes()
        {
            var finalPose = TurtlePoses
                .FirstAfter(ActionCall.ResultMessage)
                .Value;

            finalPose.AngularVelocity.Should().Be(0);
            finalPose.LinearVelocity.Should().Be(0);
        }
    }
}