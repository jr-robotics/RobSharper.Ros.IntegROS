using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public class CallbackMessageHandler<TTopic> : ExpectationMessageHandler<TTopic>
    {
        private readonly Action<TTopic> _callback;

        public CallbackMessageHandler(Action<TTopic> callback, int priority = 50) : this(callback, null, priority)
        {
        }

        public CallbackMessageHandler(Action<TTopic> callback, CallerReference callerInfo, int priority = 50) : base(callerInfo, priority)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
        {
            _callback(message);
        }
    }
}