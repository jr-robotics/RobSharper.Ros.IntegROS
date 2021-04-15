using System;
using FluentAssertions;
using RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility;
using RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests
{
    public abstract class ScenarioIdentifierTests
    {
        protected abstract IScenarioIdentifier CreateScenarioIdentifier();

        [Fact]
        public void CanSerializeIScenarioIdentifier()
        {
            var identifier = CreateScenarioIdentifier(); 
            TestSerializationInfo.Serialize(identifier);
        }
        
        [Fact]
        public void CanDeserializeIScenarioIdentifier()
        {
            var identifier = CreateScenarioIdentifier();

            var serialized = TestSerializationInfo.Serialize(identifier);
            var deserializedIdentifier = (IScenarioIdentifier) serialized.Deserialize();

            deserializedIdentifier.Should().NotBeNull();
            deserializedIdentifier.Should().BeEquivalentTo(identifier);
        }
    }

    public class RosBagScenarioIdentifierTests : ScenarioIdentifierTests
    {
        protected override IScenarioIdentifier CreateScenarioIdentifier()
        {
            return new RosbagScenarioIdentifier("MYBAGDILE.bag", typeof(int), "display name");
        }
    }

    public class DummyScenarioIdentifierTests : ScenarioIdentifierTests
    {
        protected override IScenarioIdentifier CreateScenarioIdentifier()
        {
            return new DummyScenarioIdentifier("display name");
        }
    }
    
    public class ScenarioIdentifierBaseTests : ScenarioIdentifierTests
    {
        public class TestScenarioIdentifier : ScenarioIdentifierBase
        {
            public const string UniqueId = "E3DA3555-28B4-4CA8-A32D-D24FDEF76488";

            public static TestScenarioIdentifier Default { get; } =
                new TestScenarioIdentifier(typeof(int), "My display name");
            
            
            [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
            public TestScenarioIdentifier() : base() {}
            
            public TestScenarioIdentifier(Type scenarioDiscovererType, string displayName = null) : base(
                scenarioDiscovererType, displayName)
            {
                
            }

            protected override string GetUniqueId()
            {
                return UniqueId;
            }
        }

        protected override IScenarioIdentifier CreateScenarioIdentifier()
        {
            return TestScenarioIdentifier.Default;
        }
    }
}