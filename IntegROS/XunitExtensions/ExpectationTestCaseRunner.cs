using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    [Obsolete("Unused", true)]
    public class ExpectationTestCaseRunner : TestCaseRunner<ExpectationTestCase>
    {
        private readonly string _displayName;
        private readonly ForScenario _scenario;

        public ExpectationTestCaseRunner(ForScenario scenario,
                                         ExpectationTestCase testCase,
                                         string displayName,
                                         IMessageBus messageBus,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
            : base(testCase, messageBus, aggregator, cancellationTokenSource)
        {
            this._scenario = scenario;
            this._displayName = displayName;
        }

        protected override Task<RunSummary> RunTestAsync()
        {
            var timer = new ExecutionTimer();
            var testClass = TestCase.TestMethod.TestClass.Class.ToRuntimeType();
            var testMethod = TestCase.TestMethod.Method.ToRuntimeMethod();
            var test = new ExpectationTest(TestCase, _displayName);

            return new ExpectationTestRunner(_scenario, test, MessageBus, timer, testClass, testMethod, Aggregator, CancellationTokenSource).RunAsync();
        }
    }
}

