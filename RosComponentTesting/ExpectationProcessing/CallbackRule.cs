using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public class CallbackRule<TTopic> : ExpectationRule<TTopic>
    {
        private readonly Action<TTopic> _callback;

        public CallbackRule(Action<TTopic> callback, int priority = 50) : this(callback, null, priority)
        {
        }

        public CallbackRule(Action<TTopic> callback, CallerReference callerInfo, int priority = 50) : base(callerInfo, priority)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
        {
            _callback(message);
        }
    }
}