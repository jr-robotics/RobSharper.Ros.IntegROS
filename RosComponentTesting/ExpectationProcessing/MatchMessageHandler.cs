using System;

namespace RosComponentTesting.ExpectationProcessing
{
    public class MatchMessageHandler<TTopic> : ExpectationMessageHandler<TTopic>
    {
        private readonly Match<TTopic> _match;

        public MatchMessageHandler(Match<TTopic> match, int priority = 100) : base(priority)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
        }
        
        public override void OnActivateExpectation()
        {
        }

        public override void OnDeactivateExpectation()
        {
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
        {
            context.Continue = _match.Evaluate(message);
        }
    }
}