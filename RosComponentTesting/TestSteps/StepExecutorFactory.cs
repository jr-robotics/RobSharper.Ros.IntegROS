using System;

namespace RosComponentTesting.TestSteps
{
    public class StepExecutorFactory
    {
        public ITestStepExecutor CreateExecutor<TTestStep>(TTestStep step) where TTestStep : ITestStep
        {
            if (step is ITestStepExecutor stepExecutor)
            {
                return stepExecutor;
            }
            
            throw new NotSupportedException("Step is no ITestStepExecutor (external executors are not supported yet).");
        }
    }
}