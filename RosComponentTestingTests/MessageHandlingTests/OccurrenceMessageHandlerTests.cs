using FluentAssertions;
using RosComponentTesting;
using RosComponentTesting.MessageHandling;
using Xunit;

namespace RosComponentTestingTests.MessageHandlingTests
{
    public class OccurrenceMessageHandlerTests
    {
        [Fact]
        public void IsValid_is_false_if_called_less_then_the_specified_times()
        {
            var target = new OccurrenceMessageHandler<object>(Times.Once);
            target.Activate();

            target.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public void IsValid_is_false_if_not_called_more_ofthen_than_the_specified_times()
        {
            var target = new OccurrenceMessageHandler<object>(Times.Once);
            target.Activate();
            
            target.HandleMessage(new object(), new MessageHandlingContext());
            target.HandleMessage(new object(), new MessageHandlingContext());

            target.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public void Validation_state_is_true_if_not_called_the_specified_times()
        {
            var target = new OccurrenceMessageHandler<object>(Times.Once);
            target.Activate();
            target.HandleMessage(new object(), new MessageHandlingContext());

            target.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public void ValidationState_is_not_determined_if_called_less_than_the_specified_times()
        {
            var target = new OccurrenceMessageHandler<object>(Times.Once);
            target.Activate();

            target.ValidationState.Should().Be(ValidationState.NotYetDetermined);
        }
        
        [Fact]
        public void ValidationState_is_not_determined_if_called_the_specified_times()
        {
            var target = new OccurrenceMessageHandler<object>(Times.Once);
            target.Activate();

            target.HandleMessage(new object(), new MessageHandlingContext());
            
            target.ValidationState.Should().Be(ValidationState.NotYetDetermined);
        }
        
        [Fact]
        public void ValidationState_is_stable_if_called_more_ofthen_than_the_specified_times()
        {
            var target = new OccurrenceMessageHandler<object>(Times.Once);
            target.Activate();

            target.HandleMessage(new object(), new MessageHandlingContext());
            target.HandleMessage(new object(), new MessageHandlingContext());
            
            target.ValidationState.Should().Be(ValidationState.Stable);
        }
    }
}