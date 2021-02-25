using System.Linq;
using FluentAssertions;

namespace IntegROS.Test.Expectations
{
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciWithoutCalls)] // We need a valid scenario
    public class InvalidActionMessagesTests : ForScenario
    {
        [ExpectThat]
        public void Has_Action()
        {
            Scenario.Messages.HasAction("/unknownAction").Should().BeFalse();
        }
        
        [ExpectThat]
        public void Action_Exists()
        {
            var exists = Scenario
                .Messages
                .ForAction("/unknownAction")
                .Exists;
                
            exists.Should().BeFalse();
        }
        
        [ExpectThat]
        public void Can_get_action_name()
        {
            var actionName = Scenario
                .Messages
                .ForAction("/unknownAction")
                .ActionName;

            actionName.Should().Be("/unknownAction");
        }
        
        [ExpectThat]
        public void Has_status_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/unknownAction")
                .StatusMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().BeEmpty();
        }

        [ExpectThat]
        public void Has_no_goal_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/unknownAction")
                .GoalMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().BeEmpty();
        }

        [ExpectThat]
        public void Has_no_feedback_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/unknownAction")
                .FeedbackMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().BeEmpty();
        }

        [ExpectThat]
        public void Has_no_result_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/unknownAction")
                .ResultMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().BeEmpty();
        }

        [ExpectThat]
        public void Has_no_cancel_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/unknownAction")
                .CancelMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().BeEmpty();
        }
    }
}