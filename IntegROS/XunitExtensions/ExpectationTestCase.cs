using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    [Obsolete("Unused", true)]
    public class ExpectationTestCase : TestMethodTestCase
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public ExpectationTestCase() { }

        public ExpectationTestCase(TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod)
            : base(defaultMethodDisplay, defaultMethodDisplayOptions, testMethod) { }

        protected override void Initialize()
        {
            base.Initialize();

            DisplayName = String.Format("{0}, it {1}", TestMethod.TestClass.Class.Name, TestMethod.Method.Name).Replace('_', ' ');
        }

        public Task<RunSummary> RunAsync(ForScenario scenario,
                                         IMessageBus messageBus,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
        {
            return new ExpectationTestCaseRunner(scenario, this, DisplayName, messageBus, aggregator, cancellationTokenSource).RunAsync();
        }
    }
}