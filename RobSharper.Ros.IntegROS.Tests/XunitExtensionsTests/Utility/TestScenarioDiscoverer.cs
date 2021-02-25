using System;
using RobSharper.Ros.IntegROS.Scenarios;
using RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class TestScenarioDiscoverer : IScenarioDiscoverer
    {
        public IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute)
        {
            var testScenarioAttribute = (scenarioAttribute as ReflectionAttributeInfo)?.Attribute as TestScenarioAttribute;
            
            var key = testScenarioAttribute?.Key ?? Guid.NewGuid().ToString("D");
            var behavior = testScenarioAttribute != null
                ? testScenarioAttribute.DiscoveryBehavior
                : TestScenarioDiscoveryBehavior.Default;
            
            return new TestScenarioIdentifier(key, behavior);
        }

        public IScenario GetScenario(IScenarioIdentifier scenarioIdentifier)
        {
            var si = (TestScenarioIdentifier) scenarioIdentifier;

            switch (si.DiscoveryBehavior)
            {
                case TestScenarioDiscoveryBehavior.Null:
                    return null;
                case TestScenarioDiscoveryBehavior.Exception:
                    throw new InvalidOperationException("Scenario discovery throws exception");
                case TestScenarioDiscoveryBehavior.Default:
                default:
                    return new TestScenario();
            };
        }
    }
}