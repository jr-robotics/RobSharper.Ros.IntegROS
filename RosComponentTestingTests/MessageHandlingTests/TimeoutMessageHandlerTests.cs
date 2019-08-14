using System;
using System.Threading;
using FluentAssertions;
using RosComponentTesting.MessageHandling;
using Xunit;

namespace RosComponentTestingTests.MessageHandlingTests
{
    public class TimeoutMessageHandlerTests
    {
        [Fact]
        public void Message_within_timout_sets_continue_to_true()
        {
            var context = new MessageHandlingContext();

            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new TimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().BeTrue();
        }
        
        [Fact]
        public void Message_after_timout_sets_continue_to_false()
        {
            var context = new MessageHandlingContext();

            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new TimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            Thread.Sleep(timeout);
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().BeFalse();
        }
        
        [Fact]
        public void Timeout_starts_with_activation()
        {
            var context = new MessageHandlingContext();

            var timeout = TimeSpan.FromMilliseconds(200);
            var target = new TimeoutMessageHandler<object>(timeout);
            
            target.Activate();
            Thread.Sleep(timeout);
            
            target.Deactivate();
            target.Activate();
            target.HandleMessage(new object(), context);
            
            context.Continue.Should().BeTrue();
        }
    }
}