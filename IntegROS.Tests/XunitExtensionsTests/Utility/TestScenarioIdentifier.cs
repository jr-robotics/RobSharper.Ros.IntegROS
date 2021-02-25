using System;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit.Abstractions;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class TestScenarioIdentifier : DummyScenarioIdentifier
    {
        public TestScenarioDiscoveryBehavior DiscoveryBehavior { get; set; }

        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public TestScenarioIdentifier() {}
        
        public TestScenarioIdentifier(string id, TestScenarioDiscoveryBehavior testScenarioDiscoveryBehavior) : base(id)
        {
            DiscoveryBehavior = testScenarioDiscoveryBehavior;
        }

        public override void Serialize(IXunitSerializationInfo info)
        {
            base.Serialize(info);
            
            info.AddValue(nameof(DiscoveryBehavior), (int)DiscoveryBehavior);
        }

        public override void Deserialize(IXunitSerializationInfo info)
        {
            var behavior = info.GetValue<int>(nameof(DiscoveryBehavior));
            DiscoveryBehavior = (TestScenarioDiscoveryBehavior) behavior;
            
            base.Deserialize(info);
        }

        public override Type ScenarioDiscovererType
        {
            get => typeof(TestScenarioDiscoverer);
        }
    }
}