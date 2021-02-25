using System;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery
{
    public interface IScenarioIdentifier : IXunitSerializable
    {
        public string DisplayName { get; }
        
        public string UniqueScenarioId { get; }
        
        public Type ScenarioDiscovererType { get; }
    }
}