using System.Collections.Generic;
using RosComponentTesting.TestSteps;

namespace RosComponentTesting.RosNet
{
    public class RosTestExecutorFactory : ITestExecutorFactory
    {
        public ITestExecutor Create(IEnumerable<ITestStep> steps, IEnumerable<IExpectation> expectations)
        {
            return new RosTestExecutor(steps, expectations);
        }
    }
}