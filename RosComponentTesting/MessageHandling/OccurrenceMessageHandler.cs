using System;
using System.Text;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class OccurrenceMessageHandler<TTopic> : MessageHandlerBase<TTopic>, IValidationMessageHandler
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

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            _counter++;
        }

        public bool IsValid
        {
            get { return _times.IsValid(_counter); }
        }

        public ValidationState ValidationState
        {
            get
            {
                if (_counter > _times.Max)
                {
                    // State will always be invalid because counter is larger than max
                    // and future increments will not validate again
                    return ValidationState.Stable;
                }

                return ValidationState.NotYetDetermined;
            }
        }

        public void Validate(ValidationContext context)
        {
            if (IsValid) return;

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

            context.AddError(errorMessage.ToString(), CallerInfo);
        }
    }
}