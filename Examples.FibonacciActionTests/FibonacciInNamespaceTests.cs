using FluentAssertions;
using RobSharper.Ros.IntegROS;

namespace Examples.FibonacciActionTests
{
    
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciIn2Namespaces)]
    public class FibonacciInNamespaceTests : ForScenario
    {
        [ExpectThat]
        public void ActionCalls_are_always_global_without_namespace()
        {
            /*
             * 1) Select all Messages
             * 2) Filter for any (namespace independent) "fibonacci" action calls
             */
            var fibonacciActionMessages = Scenario
                .Messages
                .ForAction("**/fibonacci")
                .Calls();

            fibonacciActionMessages.Should().NotBeEmpty();

            /*
             * Action name should be a global name
             * Action name should not have a placeholder
             * All messages belong to the actionName namespace
             */
            foreach (var actionCall in fibonacciActionMessages)
            {
                var actionName = actionCall.ActionName;

                RosNameRegex.IsGlobalPattern(actionName).Should().BeTrue();
                RosNameRegex.ContainsPlaceholders(actionName).Should().BeFalse();
                
                actionCall.GoalMessage.Topic.Should().StartWith(actionName);
                actionCall.ResultMessage?.Topic.Should().StartWith(actionName);
                actionCall.FeedbackMessages.Should().OnlyContain(x => x.Topic.StartsWith(actionName));
            }
        }
        
        [ExpectThat]
        public void Scenario_has_fibonacci_actions_in_different_namespaces()
        {
            Scenario.Messages.HasAction("/fibonacci").Should().BeFalse();
            Scenario.Messages.HasAction("/f1/fibonacci").Should().BeTrue();
            Scenario.Messages.HasAction("/f2/fibonacci").Should().BeTrue();
            Scenario.Messages.HasAction("/*/fibonacci").Should().BeTrue();
            Scenario.Messages.HasAction("**/fibonacci").Should().BeTrue();
        }

        [ExpectThat]
        public void Can_combine_ForAction_with_InNamespace()
        {
            var action = Scenario.Messages
                .InNamespace("/f1")
                .ForAction("fibonacci");

            action.Exists.Should().BeTrue();
            action.Should().OnlyContain(m => m.Topic.StartsWith("/f1/fibonacci/"));
        }

        [ExpectThat]
        public void Can_combine_ForAction_with_GroupByNamespace()
        {
            var fibonacciNamespaces = Scenario.Messages
                .GroupByNamespace("/f*");

            foreach (var namespaceMessages in fibonacciNamespaces)
            {
                var action = namespaceMessages
                    .ForAction("fibonacci");
                
                action.Exists.Should().BeTrue();
                action.Should().OnlyContain(m => m.Topic.StartsWith(namespaceMessages.Key + "/fibonacci/"));
            }
        }
    }
}