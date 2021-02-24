using System.Linq;
using Examples.FibonacciActionTests.Helpers;
using Examples.FibonacciActionTests.Messages;
using FluentAssertions;
using IntegROS;
using IntegROS.Ros.Messages;

namespace Examples.FibonacciActionTests
{
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci5)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonacci20)]
    [RosbagScenario(FibonacciActionServerBagFiles.Fibonaccis)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciCancel)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciPreempted)]
    [RosbagScenario(FibonacciActionServerBagFiles.FibonacciSuccessfulAndPreempted)]
    public class FibonacciActionResultTests : ForScenario
    {
        [ExpectThat]
        public void First_fibonacci_number_is_0()
        {
            /*
             * 1) Select all Messages
             * 2) Filter for /fibonacci action calls
             * 3) Select the result sequence of each action call
             * 4) Remove any null values or sequences without elements
             *    (may be the case if scenario has no GoalResult message for any reason)
             * 5) Select the first number of each result
             */
            var firstNumbers = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.ResultMessage.Value.Result<FibonacciResult>()?.Sequence)
                .Where(x => x != null && x.Any())
                .Select(x => x.First());

            // Now there should be only one 0 value left
            foreach (var firstNumber in firstNumbers)
            {
                firstNumber.Should().Be(0);
            }
        }
        
        [ExpectThat]
        public void Second_fibonacci_number_is_1()
        {
            /*
             * 1) Select all Messages
             * 2) Filter for /fibonacci action calls
             * 3) Select the result sequence of each action call
             * 4) Remove any null values or sequences with less than 2 elements
             *    (may be the case if scenario has no GoalResult message for any reason)
             * 5) Select the second number of each result
             */
            var secondNumbers = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.ResultMessage.Value.Result<FibonacciResult>()?.Sequence)
                .Where(x => x != null && x.Count() > 1)
                .Select(x => x.Skip(1).First());

            // Now there should be only values == 1 left
            foreach (var secondNumber in secondNumbers)
            {
                secondNumber.Should().Be(1);
            }
        }

        [ExpectThat]
        public void Each_fibonacci_number_is_the_sum_of_its_last_2_predecessors()
        {
            /*
             * Select all result sequences with at least 3 elements
             */
            var sequences = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.ResultMessage.Value.Result<FibonacciResult>()?.Sequence)
                .Where(x => x != null && x.Count() > 2);

            foreach (var sequence in sequences)
            {
                var expectedFibonacciSequence = Fibonacci.CalculateSequence(sequence.Count());

                sequence.Should().BeEquivalentTo(expectedFibonacciSequence);
            }
        }
        
        [ExpectThat]
        public void All_results_contain_valid_fibonacci_sequences()
        {
            /*
             * This is an alternative to the 3 tests before.
             * It checks if all result sequences represent a valid fibonacci row.
             */
            
            //Select all result sequences
            var sequences = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Select(x => x.ResultMessage.Value.Result<FibonacciResult>()?.Sequence)
                .Where(x => x != null);

            foreach (var sequence in sequences)
            {
                var expectedFibonacciSequence = Fibonacci.CalculateSequence(sequence.Count());

                sequence.Should().BeEquivalentTo(expectedFibonacciSequence);
            }
        }
        
        [ExpectThat]
        public void Result_sequence_has_length_specified_in_goal()
        {
            /*
             * Select all messages
             * Select action calls for fibonacci action
             * Select only succeeded action calls
             * Select goal order and result sequence
             */
            var actionCalls = Scenario
                .Messages
                .ForActionCalls("/fibonacci")
                .Where(x => x.FinalState == GoalStatusValue.Succeeded)
                .Select(x => new
                {
                    GoalOrder = x.GoalMessage.Value.Goal<FibonacciGoal>().Order,
                    ResultMessage = x.ResultMessage.Value.Result<FibonacciResult>().Sequence
                });

            // Every action call should have goal order + 2 (first two fibonaccis 0 & 1) elements 
            foreach (var actionCall in actionCalls)
            {
                var expectedMaxFeedbackMessages = actionCall.GoalOrder + 2;
                actionCall.ResultMessage.Should().HaveCount(expectedMaxFeedbackMessages);
            }
        }
    }
}