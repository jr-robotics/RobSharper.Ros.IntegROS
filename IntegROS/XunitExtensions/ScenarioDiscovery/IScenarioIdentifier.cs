using System;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public interface IScenarioIdentifier : IXunitSerializable
    {
        public string UniqueId { get; }
        
        public Type ScenarioDiscovererType { get; }
    }
}