using System;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public class DummyScenarioIdentifier : IScenarioIdentifier
    {
        public DummyScenarioIdentifier(string displayName)
        {
            DisplayName = displayName;
            UniqueScenarioId = $"DUMMYSCENARIO:{HashCode.Combine(DisplayName)}";
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            DisplayName = info.GetValue<string>(nameof(DisplayName));
            UniqueScenarioId = info.GetValue<string>(nameof(UniqueScenarioId));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(DisplayName), DisplayName);
            info.AddValue(nameof(UniqueScenarioId), UniqueScenarioId);
        }

        public string DisplayName { get; private set; }
        public string UniqueScenarioId { get; private set; }
        public Type ScenarioDiscovererType
        {
            get => typeof(DummyScenarioDiscoverer);
        }
    }
}