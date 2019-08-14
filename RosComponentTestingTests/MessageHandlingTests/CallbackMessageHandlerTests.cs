using FluentAssertions;
using RosComponentTesting.MessageHandling;
using Xunit;

namespace RosComponentTestingTests.MessageHandlingTests
{
    public class CallbackMessageHandlerTests
    {
        [Fact]
        public void Callback_is_executed()
        {
            var callbackCalled = false;
            
            var target = new CallbackMessageHandler<object>(o => { callbackCalled = true; });

            target.Activate();
            target.HandleMessage(new object(), new MessageHandlingContext());
            
            callbackCalled.Should().BeTrue();
        }
        
        [Fact]
        public void Callback_is_executed_with_message()
        {
            var callbackCalled = false;
            var expectedMessage = new object();
            
            var target = new CallbackMessageHandler<object>(o => { o.Should().Be(expectedMessage); });

            target.Activate();
            target.HandleMessage(expectedMessage, new MessageHandlingContext());
        }
    }
}