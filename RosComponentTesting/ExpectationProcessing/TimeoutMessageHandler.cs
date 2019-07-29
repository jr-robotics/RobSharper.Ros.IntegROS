using System;

namespace RosComponentTesting.ExpectationProcessing
{
    public class TimeoutMessageHandler<TTopic> : ExpectationMessageHandler<TTopic>
    {
        private readonly TimeSpan _timeout;
        private DateTime _timedOutAt;

        public TimeoutMessageHandler(TimeSpan timeout, int priority = 100) : base(priority)
        {
            _timeout = timeout;
        }

        public override void OnActivateExpectation()
        {
            _timedOutAt = DateTime.Now + _timeout;
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
        {
            context.Continue = _timedOutAt >= DateTime.Now;
        }
    }
}