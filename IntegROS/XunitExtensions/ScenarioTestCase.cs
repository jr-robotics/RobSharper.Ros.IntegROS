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
    public class ScenarioTestCase : XunitTestCase
    {
        private IScenarioIdentifier _scenarioIdentifier;
        private string _skipReason;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public ScenarioTestCase() {}

        public ScenarioTestCase(IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            IScenarioIdentifier scenarioIdentifier,
            string skipReason = null) : base(diagnosticMessageSink, defaultMethodDisplay,
            defaultMethodDisplayOptions, testMethod,
            null)
        {
            _scenarioIdentifier = scenarioIdentifier;
            _skipReason = skipReason;
        }

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

        /// <inheritdoc/>
        protected override void Initialize()
        {
            base.Initialize();

            Traits.Add("Scenario", new List<string>() {_scenarioIdentifier.ToString()});
        }

        /// <inheritdoc/>
        protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
        {
            var baseDisplayName = base.GetDisplayName(factAttribute, displayName);
            return $"{baseDisplayName} (Scenario: {_scenarioIdentifier})";
        }

        /// <inheritdoc/>
        protected override string GetUniqueID()
        {
            var scenarioId = (uint) HashCode.Combine(_scenarioIdentifier.UniqueScenarioId);
            return $"{base.GetUniqueID()}-{scenarioId}";
        }

        /// <inheritdoc/>
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            
            data.AddValue(nameof(ScenarioIdentifier), _scenarioIdentifier);
            _skipReason = data.GetValue<string>("SkipReason");
        }

        /// <inheritdoc/>
        public override void Deserialize(IXunitSerializationInfo data)
        {
            _skipReason = data.GetValue<string>("SkipReason");
            _scenarioIdentifier = data.GetValue<IScenarioIdentifier>(nameof(ScenarioIdentifier));
            
            base.Deserialize(data);
        }

        /// <inheritdoc/>
        protected override string GetSkipReason(IAttributeInfo factAttribute)
        {
            return _skipReason;
        }

        /// <inheritdoc/>
        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runner = new ScenarioTestCaseRunner(this, DisplayName, SkipReason, constructorArguments,
                diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource);

            return runner.RunAsync();
        }
    }
}