using System;

namespace RosComponentTesting.MessageHandling
{
    public class TimeoutMessageHandler<TTopic> : MessageHandlerBase<TTopic>
    {
        private readonly TimeSpan _timeout;
        private DateTime _timedOutAt;

        public TimeoutMessageHandler(TimeSpan timeout, int priority = 100) : base(priority)
        {
            _timeout = timeout;
        }
        
        public override void Activate()
        {
            _timedOutAt = DateTime.Now + _timeout;
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            context.Continue = _timedOutAt < DateTime.Now;
        }
    }
}