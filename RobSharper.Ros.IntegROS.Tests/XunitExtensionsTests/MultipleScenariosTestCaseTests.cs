using FluentAssertions;
using RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility;
using RobSharper.Ros.IntegROS.XunitExtensions;
using Xunit;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests
{
    public class MultipleScenariosTestCaseTests
    {
        private static MultipleScenariosTestCase CreateTestCase()
        {
            var testMethod = XunitMocks.TestMethod(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_scenario));
            var scenarioIdentifier = TestScenarioDiscoverer.CreateScenarioIdentifier(testMethod);

            var target = new MultipleScenariosTestCase(new NullMessageSink(), TestMethodDisplay.ClassAndMethod,
                TestMethodDisplayOptions.All, testMethod, null);
            
            return target;
        }
        
        [Fact]
        public void CanSerializeTestCaseWithoutErrorMessage()
        {
            var target = CreateTestCase();

            TestSerializationInfo.Serialize(target);
        }

        [Fact]
        public void CanDeserializeTestCaseWithoutErrorMessage()
        {
            var target = CreateTestCase();

            var serialized = TestSerializationInfo.Serialize(target);
            var deserialized = (MultipleScenariosTestCase) serialized.Deserialize();

            deserialized.Should().NotBeNull();
            deserialized.Should().BeEquivalentTo(target);
        }
    }
}