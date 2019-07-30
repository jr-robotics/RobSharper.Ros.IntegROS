using System;
using System.Threading;

namespace RosComponentTesting.TestSteps
{
    public class WaitStep : ITestStep, ITestStepExecutor
    {
        private readonly TimeSpan _duration;

        public WaitStep(TimeSpan duration)
        {
            _duration = duration;
        }

        public void Execute(IServiceProvider serviceProvider, CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Token.WaitHandle.WaitOne(_duration);
        }
    }
}