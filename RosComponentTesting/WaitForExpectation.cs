using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RosComponentTesting.MessageHandling;

namespace RosComponentTesting
{
    
    public class WaitForExpectation<TTopic> : TopicExpectation<TTopic>
    {
        private readonly AutoResetEvent _waitHandle;

        public WaitHandle WaitHandle => _waitHandle;

        public WaitForExpectation()
        {
            _waitHandle = new AutoResetEvent(false);
        }

        protected override void HandleMessageInternal(TTopic message, IEnumerable<MessageHandlerBase<TTopic>> handlers)
        {
            var context = new MessageHandlingContext();
            var calledHandlers = 0;
                
            foreach (var handler in handlers)
            {
                handler.HandleMessage(message, context);
                calledHandlers++;

                if (!context.Continue)
                    break;
            }

            var allHandlersCalled = calledHandlers == handlers.Count();
            
            // Signal if expectation met
            if (allHandlersCalled && IsValid)
            {
                _waitHandle.Set();
            }
            
            // Signal if expectation can't be met any more
            if (handlers
                .OfType<IValidationMessageHandler>()
                .Any(h => h.ValidationState == ValidationState.Stable && !h.IsValid))
            {
                _waitHandle.Set();
            }
        }
        

        public override void Cancel()
        {
            try
            {
                base.Cancel();
            }
            finally
            {
                // Signal if expectation canceled
                _waitHandle.Set();
            }
        }
    }
}