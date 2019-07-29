using System;
using System.Text;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public class OccurrenceMessageHandler<TTopic> : ExpectationMessageHandler<TTopic>, IValidationRule
    {
        private long _counter;
        private readonly Times _times;

        public OccurrenceMessageHandler(Times times, int priority = 50) : this(times, null, priority)
        {
        }

        public OccurrenceMessageHandler(Times times, CallerReference callerInfo, int priority = 50) : base(callerInfo, priority)
        {
            _times = times ?? throw new ArgumentNullException(nameof(times));
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
        {
            _counter++;
        }

        public void Validate(ValidationContext context)
        {
            if (_times.IsValid(_counter)) return;

            var errorMessage = new StringBuilder();

            errorMessage.AppendLine("Occurrences() failure");
            errorMessage.Append("Expected: ");
            
            if (_times.Min == _times.Max)
            {
                errorMessage.AppendLine($"{_times.Min}");
            }
            else if (_times.Min == 0) // at most
            {
                errorMessage.AppendLine($"<= {_times.Max}");
            }
            else if (_times.Max == uint.MaxValue) // at least
            {
                errorMessage.AppendLine($">= {_times.Min}");
            }
            else
            {
                errorMessage.AppendLine($"{_times.Min} - {_times.Max}");
            }

            errorMessage.AppendLine($"Actual:   {_counter}");

            if (CallerInfo != null)
            {
                errorMessage .AppendLine($"  in {CallerInfo}");
            }

            context.AddError(errorMessage.ToString());
        }
    }
}