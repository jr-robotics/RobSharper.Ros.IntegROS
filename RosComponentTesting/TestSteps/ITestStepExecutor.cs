using System;

namespace RosComponentTesting.TestSteps
{
    public interface ITestStepExecutor
    {
        void Execute(IServiceProvider serviceProvider);
    }
}