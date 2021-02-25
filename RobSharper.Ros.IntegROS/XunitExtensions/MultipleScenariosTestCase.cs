using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.XunitExtensions
{
    public class MultipleScenariosTestCase : XunitTestCase
    {
        protected internal new TestMethodDisplay DefaultMethodDisplay => base.DefaultMethodDisplay;
        protected internal new TestMethodDisplayOptions DefaultMethodDisplayOptions => base.DefaultMethodDisplayOptions;
        

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public MultipleScenariosTestCase() {}

        public MultipleScenariosTestCase(IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            object[] testMethodArguments = null) : base(diagnosticMessageSink, defaultMethodDisplay,
            defaultMethodDisplayOptions, testMethod,
            testMethodArguments)
        {
        }
        
        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runner = new MultipleScenariosTestCaseRunner(this, DisplayName, SkipReason, constructorArguments,
                diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource);

            return runner.RunAsync();
        }
    }
}