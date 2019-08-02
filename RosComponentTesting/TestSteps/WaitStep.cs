using System;
using System.Threading;

namespace RosComponentTesting.TestSteps
{
    public class WaitStep : ITestStep, ITestStepExecutor
    {
        private readonly TimeSpan _duration;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public WaitStep(TimeSpan duration)
        {
            _duration = duration;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            _cancellationTokenSource.Token.WaitHandle.WaitOne(_duration);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}