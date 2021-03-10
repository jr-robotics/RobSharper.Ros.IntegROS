using System;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery
{
    public interface IScenarioIdentifier : IXunitSerializable
    {
        string DisplayName { get; }
        
        string UniqueScenarioId { get; }
        
        Type ScenarioDiscovererType { get; }
    }
}