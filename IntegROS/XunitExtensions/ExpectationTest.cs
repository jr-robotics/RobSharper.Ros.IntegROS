using Xunit;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTest : LongLivedMarshalByRefObject, ITest
    {
        public ExpectationTest(ExpectationTestCase testCase, string displayName)
        {
            TestCase = testCase;
            DisplayName = displayName;
        }

        public string DisplayName { get; private set; }

        public ExpectationTestCase TestCase { get; private set; }

        ITestCase ITest.TestCase { get { return TestCase; } }
    }
}
