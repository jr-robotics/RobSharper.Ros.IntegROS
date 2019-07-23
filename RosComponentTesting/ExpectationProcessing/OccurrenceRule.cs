using System;

namespace RosComponentTesting.ExpectationProcessing
{
    public class OccurrenceRule<TTopic> : ExpectationRule<TTopic>, IValidationRule
    {
        private long _counter;
        private readonly Times _times;

        public OccurrenceRule(Times times, int priority = 50) : base(priority)
        {
            _times = times ?? throw new ArgumentNullException(nameof(times));
        }
        
        public override void OnActivateExpectation()
        {
        }

        public override void OnDeactivateExpectation()
        {
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
        {
            _counter++;
        }

        public void Validate(ValidationContext context)
        {
            if (_times.IsValid(_counter)) return;
            
            
            string errorMessage;
                
            if (_times == Times.Never)
            {
                errorMessage = $"No messages expected, but received {_counter}.";
            }
            else if (_times == Times.Once)
            {
                errorMessage = $"One message expected, but received {_counter}.";
            }
            else if (_times.Min == 0) // at most
            {
                errorMessage = $"At most {_times.Max} messages expected, but received {_counter}.";
            }
            else if (_times.Max == uint.MaxValue) // at least
            {
                errorMessage = $"At least {_times.Min} messages expected, but received {_counter}.";
            }
            else
            {
                errorMessage = $"Expected between {_times.Min} and {_times.Max} messages, but received {_counter}.";
            }
                
            context.AddError(errorMessage);
        }
    }
}