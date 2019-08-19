using System;
using RosComponentTesting;
using RosComponentTesting.RosNet;
using Xunit;

namespace RosComponentTestingTests
{
    public class RosTestExecutorTests
    {
        [Fact()]
        public void Cannot_initialize_with_null_expectations()
        {
            Assert.Throws<ArgumentNullException>(() => new RosTestExecutor(null, null));
        }
    }
}