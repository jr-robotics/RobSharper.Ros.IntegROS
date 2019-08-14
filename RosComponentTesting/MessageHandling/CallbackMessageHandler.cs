using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class CallbackMessageHandler<TTopic> : MessageHandlerBase<TTopic>
    {
        private readonly Action<TTopic> _callback;

        public CallbackMessageHandler(Action<TTopic> callback, int priority = 50) : this(callback, null, priority)
        {
        }

        public CallbackMessageHandler(Action<TTopic> callback, CallerReference callerInfo, int priority = 50) : base(callerInfo, priority)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            _callback(message);
        }
    }
}