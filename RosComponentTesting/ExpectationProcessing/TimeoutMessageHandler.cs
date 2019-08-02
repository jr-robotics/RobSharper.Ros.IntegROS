using System;
using System.Threading;
using System.Threading.Tasks;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.ExpectationProcessing
{
    public class TimeoutMessageHandler<TTopic> : ExpectationMessageHandler<TTopic>, IValidationRule
    {
        private readonly TimeSpan _timeout;
        private readonly TimeoutBehaviour _behaviour;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _timedOut;

        public TimeoutMessageHandler(TimeSpan timeout, TimeoutBehaviour behaviour, int priority = 100) : this(timeout, behaviour, null, priority)
        {
            _timeout = timeout;
            _behaviour = behaviour;
        }
        
        public TimeoutMessageHandler(TimeSpan timeout, TimeoutBehaviour behaviour, CallerReference callerInfo, int priority = 100) : base(callerInfo, priority)
        {
            _timeout = timeout;
            _behaviour = behaviour;
        }

        public override void OnActivateExpectation()
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

        public override void OnDeactivateExpectation()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }

        public override void OnHandleMessage(TTopic message, ExpectationRuleContext context)
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
            if (_timedOut && _behaviour == TimeoutBehaviour.ThrowValidationError)
            {
                context.AddError("Expectation could not be met in time", CallerInfo);
            }
        }
    }
}