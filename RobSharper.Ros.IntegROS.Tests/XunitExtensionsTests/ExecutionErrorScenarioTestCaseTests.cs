using FluentAssertions;
using RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility;
using RobSharper.Ros.IntegROS.XunitExtensions;
using Xunit;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests
{
    public class ExecutionErrorScenarioTestCaseTests
    {
        private static ExecutionErrorScenarioTestCase CreateTestCase(string errorMessage = null)
        {
            var testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_scenario));
            var scenarioIdentifier = TestScenarioDiscoverer.CreateScenarioIdentifier(testMethod);

            var target = new ExecutionErrorScenarioTestCase(new NullMessageSink(), TestMethodDisplay.ClassAndMethod,
                TestMethodDisplayOptions.All, testMethod, scenarioIdentifier, errorMessage);
            
            return target;
        }
        
        [Fact]
        public void CanSerializeTestCaseWithoutErrorMessage()
        {
            var target = CreateTestCase();
            target.ErrorMessage.Should().BeNull();

            TestSerializationInfo.Serialize(target);
        }

        [Fact]
        public void CanDeserializeTestCaseWithoutErrorMessage()
        {
            var target = CreateTestCase();
            target.ErrorMessage.Should().BeNull();

            var serialized = TestSerializationInfo.Serialize(target);
            var deserialized = (ExecutionErrorScenarioTestCase) serialized.Deserialize();

            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(target);
        }

        [Fact]
        public void CanSerializeTestCaseWithSkipReason()
        {
            var target = CreateTestCase("ERROR MESSAGE");
            target.ErrorMessage.Should().NotBeNullOrEmpty();

            TestSerializationInfo.Serialize(target);
        }

        [Fact]
        public void CanDeserializeTestCaseWithSkipReason()
        {
            var target = CreateTestCase("ERROR MESSAGE");
            target.ErrorMessage.Should().NotBeNullOrEmpty();

            var serialized = TestSerializationInfo.Serialize(target);
            var deserialized = (ExecutionErrorScenarioTestCase) serialized.Deserialize();

            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(target);
        }
    }
}