using System.Collections.Generic;
using RosComponentTesting.TestSteps;

namespace RosComponentTesting.RosNet
{
    public class RosTestExecutorFactory : ITestExecutorFactory
    {
        private readonly string _rosMasterUri;
        private readonly string _nodeName;

        public RosTestExecutorFactory(string rosMasterUri, string nodeName)
        {
            _rosMasterUri = rosMasterUri;   
            _nodeName = nodeName;
        }

        public ITestExecutor Create(IEnumerable<ITestStep> steps, IEnumerable<IExpectation> expectations)
        {
            return new RosTestExecutor(_rosMasterUri, _nodeName, steps, expectations);
        }
    }
}