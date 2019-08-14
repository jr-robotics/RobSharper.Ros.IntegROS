using System;

namespace RosComponentTesting.MessageHandling
{
    public class CallbackMessageHandler<TTopic> : MessageHandlerBase<TTopic>
    {
        private readonly Action<TTopic> _callback;

        public CallbackMessageHandler(Action<TTopic> callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            _callback(message);
        }
    }
}