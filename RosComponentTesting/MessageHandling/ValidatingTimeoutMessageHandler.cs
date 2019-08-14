using System;
using System.Threading;
using System.Threading.Tasks;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class ValidatingTimeoutMessageHandler<TTopic> : MessageHandlerBase<TTopic>, IValidationMessageHandler
    {
        private readonly TimeSpan _timeout;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _timedOut;

        public ValidatingTimeoutMessageHandler(TimeSpan timeout, int priority = 100) : this(timeout, null, priority)
        {
        }
        
        public ValidatingTimeoutMessageHandler(TimeSpan timeout, CallerReference callerInfo, int priority = 100) : base(callerInfo, priority)
        {
            _timeout = timeout;
        }
        
        public override void Activate()
        {
            _timedOut = false;
            _cancellationTokenSource = new CancellationTokenSource();
            
            var token = _cancellationTokenSource.Token;
            
            Task.Run(() =>
            {
                if (!token.WaitHandle.WaitOne(_timeout))
                {
                    _timedOut = true;
                }
            }, token);
        }

        public override void Deactivate()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            context.Continue = !_timedOut;
        }

        public bool IsValid
        {
            get { return !_timedOut; }
        }

        public ValidationState ValidationState
        {
            get
            {
                return _timedOut ? ValidationState.Stable : ValidationState.NotYetDetermined;
            }
        }

        public void Validate(ValidationContext context)
        {
            if (!IsValid)
            {
                context.AddError("Expectation could not be met in time", CallerInfo);
            }
        }
    }
}