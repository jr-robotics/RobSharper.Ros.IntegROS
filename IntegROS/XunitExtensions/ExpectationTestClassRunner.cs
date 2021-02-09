using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTestClassRunner : TestClassRunner<ExpectationTestCase>
    {
        private readonly ForNewScenario _scenario;

        public ExpectationTestClassRunner(ForNewScenario scenario,
                                          ITestClass testClass,
                                          IReflectionTypeInfo @class,
                                          IEnumerable<ExpectationTestCase> testCases,
                                          IMessageSink diagnosticMessageSink,
                                          IMessageBus messageBus,
                                          ITestCaseOrderer testCaseOrderer,
                                          ExceptionAggregator aggregator,
                                          CancellationTokenSource cancellationTokenSource)
            : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            _scenario = scenario;
        }

        protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod,
                                                               IReflectionMethodInfo method,
                                                               IEnumerable<ExpectationTestCase> testCases,
                                                               object[] constructorArguments)
        {
            return new ExpectationTestMethodRunner(_scenario, testMethod, Class, method, testCases, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource).RunAsync();
        }
    }
}
