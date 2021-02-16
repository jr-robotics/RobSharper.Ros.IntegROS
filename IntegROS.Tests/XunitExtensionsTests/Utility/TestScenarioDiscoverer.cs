using System;
using IntegROS.Scenarios;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class TestScenarioDiscoverer : IScenarioDiscoverer
    {
        public IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute)
        {
            var key = ((scenarioAttribute as ReflectionAttributeInfo)?.Attribute as TestScenarioAttribute)?.Key;
            return new TestScenarioIdentifier(key ?? Guid.NewGuid().ToString("D"));
        }

        public IScenario GetScenario(IScenarioIdentifier scenarioIdentifier)
        {
            return new TestScenario();
        }
    }

    public class TestScenarioIdentifier : DummyScenarioIdentifier
    {
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public TestScenarioIdentifier() {}
        
        public TestScenarioIdentifier(string id) : base(id)
        {
        }

        public override Type ScenarioDiscovererType
        {
            get => typeof(TestScenarioDiscoverer);
        }
    }
}