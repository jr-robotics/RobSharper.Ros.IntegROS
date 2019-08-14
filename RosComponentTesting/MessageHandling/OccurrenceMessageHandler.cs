using System;
using System.Text;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class OccurrenceMessageHandler<TTopic> : ValidationMessageHandlerBase<TTopic>
    {
        private long _counter;
        private readonly Times _expectedOccurrences;

        public OccurrenceMessageHandler(Times expectedOccurrences) : this(expectedOccurrences, null)
        {
        }

        public OccurrenceMessageHandler(Times expectedOccurrences, CallerReference callerInfo) : base(callerInfo)
        {
            _expectedOccurrences = expectedOccurrences ?? throw new ArgumentNullException(nameof(expectedOccurrences));
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            _counter++;
        }

        public override bool IsValid
        {
            get { return _expectedOccurrences.IsValid(_counter); }
        }

        public override ValidationState ValidationState
        {
            get
            {
                if (_counter > _expectedOccurrences.Max)
                {
                    // State will always be invalid because counter is larger than max
                    // and future increments will not validate again
                    return ValidationState.Stable;
                }

                return ValidationState.NotYetDetermined;
            }
        }

        public override void Validate(ValidationContext context)
        {
            if (IsValid) return;

            var errorMessage = new StringBuilder();

            errorMessage.AppendLine("Occurrences() failure");
            errorMessage.Append("Expected: ");
            
            if (_expectedOccurrences.Min == _expectedOccurrences.Max)
            {
                errorMessage.AppendLine($"{_expectedOccurrences.Min}");
            }
            else if (_expectedOccurrences.Min == 0) // at most
            {
                errorMessage.AppendLine($"<= {_expectedOccurrences.Max}");
            }
            else if (_expectedOccurrences.Max == uint.MaxValue) // at least
            {
                errorMessage.AppendLine($">= {_expectedOccurrences.Min}");
            }
            else
            {
                errorMessage.AppendLine($"{_expectedOccurrences.Min} - {_expectedOccurrences.Max}");
            }

            errorMessage.AppendLine($"Actual:   {_counter}");

            context.AddError(errorMessage.ToString(), CallerInfo);
        }
    }
}