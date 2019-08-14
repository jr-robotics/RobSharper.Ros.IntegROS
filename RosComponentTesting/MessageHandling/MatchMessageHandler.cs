using System;

namespace RosComponentTesting.MessageHandling
{
    public class MatchMessageHandler<TTopic> : MessageHandlerBase<TTopic>
    {
        private readonly Match<TTopic> _match;

        public MatchMessageHandler(Match<TTopic> match) : base()
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            context.Continue = _match.Evaluate(message);
        }
    }
}