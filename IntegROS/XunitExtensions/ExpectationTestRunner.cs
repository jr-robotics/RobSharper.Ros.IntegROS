using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    [Obsolete("Unused", true)]
    public class ExpectationTestRunner : TestRunner<ExpectationTestCase>
    {
        private readonly ForScenario _scenario;
        private readonly ExecutionTimer _timer;
        
        public ExpectationTestRunner(ForScenario scenario,
                                     ITest test,
                                     IMessageBus messageBus,
                                     ExecutionTimer timer,
                                     Type testClass,
                                     MethodInfo testMethod,
                                     ExceptionAggregator aggregator,
                                     CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, null, testMethod, null, null, aggregator, cancellationTokenSource)
        {
            this._scenario = scenario;
            this._timer = timer;
        }

        protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
        {
            var duration = await new ExpectationTestInvoker(_scenario, Test, MessageBus, TestClass, TestMethod, aggregator, CancellationTokenSource).RunAsync();
            return Tuple.Create(duration, String.Empty);
        }
    }
}

