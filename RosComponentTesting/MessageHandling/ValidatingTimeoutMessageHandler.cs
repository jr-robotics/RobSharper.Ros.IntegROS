using System;
using System.Threading;
using System.Threading.Tasks;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public class ValidatingTimeoutMessageHandler<TTopic> : ValidationMessageHandlerBase<TTopic>
    {
        private readonly TimeSpan _timeout;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _timedOut;

        public ValidatingTimeoutMessageHandler(TimeSpan timeout) : this(timeout, null)
        {
        }
        
        public ValidatingTimeoutMessageHandler(TimeSpan timeout, CallerReference callerInfo) : base(callerInfo)
        {
            _timeout = timeout;
        }
        
        public override void Activate()
        {
            base.Activate();
            
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
            base.Deactivate();
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }

        protected override void HandleMessageInternal(TTopic message, MessageHandlingContext context)
        {
            context.Continue = !_timedOut;
        }

        public override bool IsValid
        {
            get { return !_timedOut; }
        }

        public override ValidationState ValidationState
        {
            get
            {
                return !IsActive ? ValidationState.Stable : ValidationState.NotYetDetermined;
            }
        }

        public override void Validate(ValidationContext context)
        {
            if (!IsValid)
            {
                context.AddError("Expectation could not be met in time", CallerInfo);
            }
        }
    }
}