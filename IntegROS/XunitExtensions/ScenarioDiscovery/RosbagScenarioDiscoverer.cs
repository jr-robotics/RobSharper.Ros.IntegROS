using System;
using System.Collections.Concurrent;
using System.Linq;
using IntegROS.Scenarios;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    /// <summary>
    /// Scenario discoverer for scenarios provided by <see cref="IntegROS.RosbagScenarioAttribute"/>
    /// </summary>
    public class RosbagScenarioDiscoverer : IScenarioDiscoverer
    {
        private readonly ConcurrentDictionary<string, RosbagScenario> _scenarios =
            new ConcurrentDictionary<string, RosbagScenario>();

        public IScenarioIdentifier GetScenarioIdentifier(IAttributeInfo scenarioAttribute)
        {
            var bagfile = scenarioAttribute.GetConstructorArguments().FirstOrDefault() as string;
            var displayName = scenarioAttribute.GetNamedArgument<string>(nameof(RosbagScenarioAttribute.DisplayName));
            
            if (string.IsNullOrEmpty(bagfile))
            {
                throw new InvalidOperationException("Bagfile is null or empty");
            }
            
            return new RosbagScenarioIdentifier(bagfile, this.GetType(), displayName);
        }
        
        public IScenario GetScenario(IScenarioIdentifier identifier)
        {
            var rosbagIdentifier = identifier as RosbagScenarioIdentifier;
            
            if (rosbagIdentifier == null)
            {
                throw new ArgumentException("Identifier is no RosbagIdentifier");
            }

            var scenario = _scenarios.GetOrAdd(rosbagIdentifier.Bagfile, (x) => new RosbagScenario(x));
            
            return scenario;
        }
    }
}