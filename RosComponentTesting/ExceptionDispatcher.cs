using System;
using System.Threading;

namespace RosComponentTesting
{
    public class ExceptionDispatcher
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Exception _ex;

        public bool HasError => _ex != null;

        public ExceptionDispatcher(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void Dispatch(Exception exception)
        {
            _ex = exception;
            _cancellationTokenSource.Cancel();
        }

        public void Throw()
        {
            if (_ex == null)
            {
                return;
            }

            throw _ex;
        }
    }
}