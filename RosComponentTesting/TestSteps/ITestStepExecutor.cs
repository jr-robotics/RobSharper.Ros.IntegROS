using System;
using System.Threading;

namespace RosComponentTesting.TestSteps
{
    public interface ITestStepExecutor
    {
        void Execute(IServiceProvider serviceProvider, CancellationTokenSource cancellationTokenSource);
    }
}