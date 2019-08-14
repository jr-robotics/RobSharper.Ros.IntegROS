using FluentAssertions;
using RosComponentTesting;
using RosComponentTesting.MessageHandling;
using Xunit;

namespace RosComponentTestingTests.MessageHandlingTests
{
    public class MatchMessageHandlerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HandleMessage_sets_continue_to_result_of_match_expression(bool value)
        {
            var target = new MatchMessageHandler<object>(It.Matches<object>(x => value));
            var context = new MessageHandlingContext();
            
            target.Activate();
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().Be(value);
        }
    }
}