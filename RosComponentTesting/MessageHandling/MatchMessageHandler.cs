using System;

namespace RosComponentTesting.MessageHandling
{
    public class MatchMessageHandler<TTopic> : MessageHandlerBase<TTopic>
    {
        private readonly Match<TTopic> _match;

        public MatchMessageHandler(Match<TTopic> match, int priority = 100) : base(priority)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
        }
        
        public override void Activate()
        {
        }

        public override void Deactivate()
        {
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            context.Continue = _match.Evaluate(message);
        }
    }
}