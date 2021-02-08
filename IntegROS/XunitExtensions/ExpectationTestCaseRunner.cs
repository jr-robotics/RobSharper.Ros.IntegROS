using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTestCaseRunner : TestCaseRunner<ExpectationTestCase>
    {
        readonly string displayName;
        readonly Specification specification;

        public ExpectationTestCaseRunner(Specification specification,
                                         ExpectationTestCase testCase,
                                         string displayName,
                                         IMessageBus messageBus,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
            : base(testCase, messageBus, aggregator, cancellationTokenSource)
        {
            this.specification = specification;
            this.displayName = displayName;
        }

        protected override Task<RunSummary> RunTestAsync()
        {
            var timer = new ExecutionTimer();
            var TestClass = TestCase.TestMethod.TestClass.Class.ToRuntimeType();
            var TestMethod = TestCase.TestMethod.Method.ToRuntimeMethod();
            var test = new ExpectationTest(TestCase, displayName);

            return new ExpectationTestRunner(specification, test, MessageBus, timer, TestClass, TestMethod, Aggregator, CancellationTokenSource).RunAsync();
        }
    }
}

