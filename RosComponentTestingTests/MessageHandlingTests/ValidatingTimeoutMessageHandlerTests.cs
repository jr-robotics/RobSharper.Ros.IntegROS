using System;
using System.Threading;
using FluentAssertions;
using RosComponentTesting.MessageHandling;
using Xunit;

namespace RosComponentTestingTests.MessageHandlingTests
{
    public class ValidatingTimeoutMessageHandlerTests
    {
        [Fact]
        public void Message_within_timout_sets_continue_to_true()
        {
            var context = new MessageHandlingContext();

            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().BeTrue();
        }
        
        [Fact]
        public void Message_after_timout_sets_continue_to_false()
        {
            var context = new MessageHandlingContext();

            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            Thread.Sleep(TimeSpan.FromMilliseconds(250));
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().BeFalse();
        }
        
        [Fact]
        public void Timeout_starts_with_activation()
        {
            var context = new MessageHandlingContext();

            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            Thread.Sleep(TimeSpan.FromMilliseconds(250));
            
            target.Deactivate();
            target.Activate();
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().BeTrue();
        }
        
        [Fact]
        public void IsValid_is_true_if_deactivated_in_time()
        {
            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            target.Deactivate();
            
            target.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public void IsValid_is_false_if_not_deactivated_in_time()
        {
            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            Thread.Sleep(TimeSpan.FromMilliseconds(250));
            target.Deactivate();

            target.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidationState_is_stable_if_handler_is_deactivated()
        {
            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.ValidationState.Should().Be(ValidationState.Stable);
            
            target.Activate();
            target.Deactivate();

            target.ValidationState.Should().Be(ValidationState.Stable);
        }
        
        [Fact]
        public void ValidationState_is_not_determined_if_handler_is_activated()
        {
            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new ValidatingTimeoutMessageHandler<object>(timeout);
            
            target.Activate();

            target.ValidationState.Should().Be(ValidationState.NotYetDetermined);
        }
    }
}