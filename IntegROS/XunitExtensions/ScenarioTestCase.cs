using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
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
    
    public class ScenarioTestCase : XunitTestCase
    {
        private IScenarioIdentifier _scenarioIdentifier;

        public IScenarioIdentifier ScenarioIdentifier
        {
            get
            {
                EnsureInitialized();
                return _scenarioIdentifier;
            }
            private set
            {
                EnsureInitialized();
                _scenarioIdentifier = value;
            }
        }

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
            _scenarioIdentifier = scenarioIdentifier;
        }

        protected override void Initialize()
        {
            base.Initialize();

            DisplayName += $"(scenario: \"{_scenarioIdentifier}\")";
            Traits.Add("Scenario", new List<string>() {_scenarioIdentifier.ToString()});
        }

        protected override string GetUniqueID()
        {
            return base.GetUniqueID() + $"[{_scenarioIdentifier}]";
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            
            data.AddValue(nameof(ScenarioIdentifier), _scenarioIdentifier);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            _scenarioIdentifier = data.GetValue<IScenarioIdentifier>(nameof(ScenarioIdentifier));
            
            base.Deserialize(data);
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