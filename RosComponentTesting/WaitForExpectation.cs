using System.Threading;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class WaitForExpectation<TTopic> : TopicExpectation<TTopic>
    {
        private class SignalMessageHandler : ExpectationMessageHandler<TTopic>
        {
            public bool Signal { get; set; }
            
            public SignalMessageHandler() : base(int.MinValue)
            {
            }

            public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
            {
                Signal = true;
            }
        }
        
        private readonly SignalMessageHandler _signalMessageHandler;
        private readonly AutoResetEvent _waitHandle;
        private object _gate = new object();

        public WaitHandle WaitHandle => _waitHandle;

        public WaitForExpectation()
        {
            _signalMessageHandler = new SignalMessageHandler();
            _waitHandle = new AutoResetEvent(false);
            
            AddMessageHandler(_signalMessageHandler);
        }
        
        protected override void HandleMessageInternal(TTopic message)
        {
            _signalMessageHandler.Signal = false;
            base.HandleMessageInternal(message);

            if (_signalMessageHandler.Signal && IsValid)
            {
                _waitHandle.Set();
            }
        }
    }
}