using System.Linq;
using FluentAssertions;

namespace RobSharper.Ros.IntegROS.Tests.Expectations
{
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciWithoutCalls)]
    public class NoCallActionMessagesTests : ForScenario
    {
        [ExpectThat]
        public void Has_Action()
        {
            Scenario.Messages.HasAction("/fibonacci").Should().BeTrue();
        }
        
        [ExpectThat]
        public void Action_Exists()
        {
            var exists = Scenario
                .Messages
                .ForAction("/fibonacci")
                .Exists;
                
            exists.Should().BeTrue();
        }
        
        [ExpectThat]
        public void Can_get_action_name()
        {
            var actionName = Scenario
                .Messages
                .ForAction("/fibonacci")
                .ActionNamePattern;

            actionName.Should().Be("/fibonacci");
        }
        
        [ExpectThat]
        public void Has_status_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
                .StatusMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().NotBeEmpty();

            allMessages
                .Select(x => x.Value)
                .ToList()
                .Should().NotBeEmpty();
        }

        [ExpectThat]
        public void Has_no_goal_messages()
        {
            var allMessages = Scenario
                .Messages
                .ForAction("/fibonacci")
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
                .ForAction("/fibonacci")
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
                .ForAction("/fibonacci")
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
                .ForAction("/fibonacci")
                .CancelMessages
                .ToList();

            allMessages.Should().NotBeNull();
            allMessages.Should().BeEmpty();
        }
    }
}