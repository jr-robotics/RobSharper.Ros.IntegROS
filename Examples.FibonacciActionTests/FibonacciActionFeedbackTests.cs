using System.Linq;
using Examples.FibonacciActionTests.Helpers;
using Examples.FibonacciActionTests.Messages;
using FluentAssertions;
using RobSharper.Ros.IntegROS;

namespace Examples.FibonacciActionTests
{
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted)]
    public class FibonacciActionFeedbackTests : ForScenario
    {
        [ExpectThat]
        public void All_feedback_messages_contain_a_valid_fibonacci_sequence()
        {
            /*
             * Select all feedback sequences
             */
            var sequences = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .SelectMany(x => x.FeedbackMessages)
                .Select(f => f?.Value.Feedback<FibonacciFeedback>()?.Sequence)
                .Where(x => x != null);

            foreach (var sequence in sequences)
            {
                var expectedFibonacciSequence = Fibonacci.CalculateSequence(sequence.Count());

                sequence.Should().BeEquivalentTo(expectedFibonacciSequence);
            }
        }
        
        [ExpectThat]
        public void First_feedback_has_fixed_structure()
        {
            var firstFeedbackSequences = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.FeedbackMessages.FirstOrDefault())
                .Where(x => x != null)
                .Select(x => x.Value.Feedback<FibonacciFeedback>().Sequence);
            
            foreach (var sequence in firstFeedbackSequences)
            {
                sequence.Should().BeEquivalentTo(new[] {0, 1, 1});
            }
        }

        [ExpectThat]
        public void There_are_not_more_feedback_messages_then_order_specified_in_goal()
        {
            var actionCalls = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => new
                {
                    GoalOrder = x.GoalMessage.Value.Goal<FibonacciGoal>().Order,
                    FeedbackMessages = x.FeedbackMessages
                })
                .Where(x => x.FeedbackMessages != null && x.FeedbackMessages.Any());

            foreach (var actionCall in actionCalls)
            {
                var expectedMaxFeedbackMessages = actionCall.GoalOrder;
                actionCall.FeedbackMessages.Should().HaveCountLessOrEqualTo(expectedMaxFeedbackMessages);
            }
        }

        [ExpectThat]
        public void All_feedback_messages_are_unique()
        {
            var actionFeedbackMessages = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.FeedbackMessages)
                .Where(x => x != null && x.Any());

            foreach (var feedbackMessages in actionFeedbackMessages)
            {
                feedbackMessages.Should().OnlyHaveUniqueItems();
            }
        }

        [ExpectThat]
        public void Every_feedback_message_adds_a_new_number_to_the_sequence()
        {
            var actionFeedbacks = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.FeedbackMessages?
                    .Select(f => f.Value.Feedback<FibonacciFeedback>().Sequence))
                .Where(x => x != null && x.Any());

            foreach (var actionFeedback in actionFeedbacks)
            {
                for (var i = 0; i < actionFeedback.Count(); i++)
                {
                    var expectedCount = i + 3;
                    var expectedSequence = Fibonacci.CalculateSequence(expectedCount);
                
                    var feedback = actionFeedback.Skip(i).First();

                    feedback.Should().HaveCount(expectedCount);
                    feedback.Should().BeEquivalentTo(expectedSequence);
                }
            }
        }
    }
}