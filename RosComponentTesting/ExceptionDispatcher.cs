using System;
using System.Threading;

namespace RosComponentTesting
{
    public class ExceptionDispatcher
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Exception _ex;

        public Exception Exception => _ex;

        public bool HasException => _ex != null;

        public ExceptionDispatcher(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void Dispatch(Exception exception)
        {
            _ex = exception;
            _cancellationTokenSource.Cancel();
        }
    }
}