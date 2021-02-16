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
            return new DummyScenarioIdentifier(key ?? Guid.NewGuid().ToString("D"));
        }

        public IScenario GetScenario(IScenarioIdentifier scenarioIdentifier)
        {
            return new TestScenario();
        }
    }
}