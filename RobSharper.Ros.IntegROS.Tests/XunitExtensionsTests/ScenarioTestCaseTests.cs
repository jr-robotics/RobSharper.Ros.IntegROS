using FluentAssertions;
using RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility;
using RobSharper.Ros.IntegROS.XunitExtensions;
using Xunit;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests
{
    public class ScenarioTestCaseTests
    {
        [Fact]
        public void CanSerializeTestCaseWithoutSkipReason()
        {
            var target = CreateScenarioTestCase();
            target.SkipReason.Should().BeNull();

            TestSerializationInfo.Serialize(target);
        }

        [Fact]
        public void CanDeserializeTestCaseWithoutSkipReason()
        {
            var target = CreateScenarioTestCase();
            target.SkipReason.Should().BeNull();

            var serialized = TestSerializationInfo.Serialize(target);
            var deserialized = (ScenarioTestCase) serialized.Deserialize();

            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(target);
        }

        [Fact]
        public void CanSerializeTestCaseWithSkipReason()
        {
            var target = CreateScenarioTestCase("SKIP REASON");
            target.SkipReason.Should().NotBeNullOrEmpty();

            TestSerializationInfo.Serialize(target);
        }

        [Fact]
        public void CanDeserializeTestCaseWithSkipReason()
        {
            var target = CreateScenarioTestCase("SKIP REASON");
            target.SkipReason.Should().NotBeNullOrEmpty();

            var serialized = TestSerializationInfo.Serialize(target);
            var deserialized = (ScenarioTestCase) serialized.Deserialize();

            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(target);
        }

        private static ScenarioTestCase CreateScenarioTestCase(string skipReason = null)
        {
            var testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_scenario));
            var scenarioIdentifier = TestScenarioDiscoverer.CreateScenarioIdentifier(testMethod);

            var target = new ScenarioTestCase(new NullMessageSink(), TestMethodDisplay.ClassAndMethod,
                TestMethodDisplayOptions.All, testMethod, scenarioIdentifier, skipReason);
            return target;
        }
    }
}