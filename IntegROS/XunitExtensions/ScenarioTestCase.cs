using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ScenarioTestCase : XunitTestCase
    {
        public IScenarioIdentifier ScenarioIdentifier { get; private set; }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public ScenarioTestCase() {}

        public ScenarioTestCase(IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            IScenarioIdentifier scenarioIdentifier,
            object[] testMethodArguments = null) : base(diagnosticMessageSink, defaultMethodDisplay,
            defaultMethodDisplayOptions, testMethod,
            testMethodArguments)
        {
            ScenarioIdentifier = scenarioIdentifier;
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(ScenarioIdentifier), ScenarioIdentifier);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            ScenarioIdentifier = data.GetValue<IScenarioIdentifier>(nameof(ScenarioIdentifier));
        }

        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runner = new ScenarioTestCaseRunner(this, DisplayName, SkipReason, constructorArguments,
                diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource);

            return runner.RunAsync();
        }
    }
}