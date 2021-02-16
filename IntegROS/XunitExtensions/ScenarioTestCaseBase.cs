using System;
using System.Collections.Generic;
using System.ComponentModel;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public abstract class ScenarioTestCaseBase : XunitTestCase
    {
        private IScenarioIdentifier _scenarioIdentifier;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public ScenarioTestCaseBase() {}

        public ScenarioTestCaseBase(IMessageSink diagnosticMessageSink,
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

        protected override void Initialize()
        {
            base.Initialize();

            DisplayName += $"(scenario: \"{_scenarioIdentifier}\")";
            Traits.Add("Scenario", new List<string>() {_scenarioIdentifier.ToString()});
        }

        protected override string GetUniqueID()
        {
            var scenarioId = (uint) HashCode.Combine(_scenarioIdentifier.UniqueScenarioId);
            return $"{base.GetUniqueID()}-{scenarioId}";
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
    }
}