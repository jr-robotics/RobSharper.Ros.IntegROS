using System.Collections.Generic;
using RosComponentTesting.TestSteps;

namespace RosComponentTesting
{
    public interface ITestExecutorFactory
    {
        ITestExecutor Create(IEnumerable<ITestStep> steps, IEnumerable<IExpectation> expectations);
    }
}