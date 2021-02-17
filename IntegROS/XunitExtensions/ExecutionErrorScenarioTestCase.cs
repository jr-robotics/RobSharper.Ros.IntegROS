using System;
using System.Collections.Generic;
using System.ComponentModel;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExecutionErrorScenarioTestCase : ExecutionErrorTestCase, IScenarioTestCase
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
        public ExecutionErrorScenarioTestCase() {}
        
        public ExecutionErrorScenarioTestCase(IMessageSink diagnosticMessageSink,
            TestMethodDisplay methodDisplayOrDefault, TestMethodDisplayOptions methodDisplayOptionsOrDefault,
            ITestMethod testMethod, IScenarioIdentifier scenarioIdentifier, string errorMessage)
            : base(diagnosticMessageSink, methodDisplayOrDefault, methodDisplayOptionsOrDefault, testMethod,
                errorMessage)
        {
            _scenarioIdentifier = scenarioIdentifier;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Traits.Add("Scenario", new List<string>() {_scenarioIdentifier.ToString()});
        }

        protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
        {
            var baseDisplayName = base.GetDisplayName(factAttribute, displayName);
            return $"{baseDisplayName} (Scenario: {_scenarioIdentifier})";
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