using System.Linq;
using FluentAssertions;
using RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility;
using RobSharper.Ros.IntegROS.XunitExtensions;
using Xunit;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests
{
    public class ScenarioAttributeHelpersTests
    {
        internal class TestClassWithoutClassScenarioAttribute : ForScenario
        {
            public void TestMethodWithoutScenarioAttribute()
            {
                
            }
            
            [TestScenario("MethodKey", DisplayName = "Method scenario")]
            public void TestMethodWithScenarioAttribute()
            {
                
            }
        }
        
        [TestScenario("ClassKey", DisplayName = "Class scenario")]
        internal class TestClassWithClassScenarioAttribute : ForScenario
        {
            public void TestMethodWithoutScenarioAttribute()
            {
                
            }
            
            [TestScenario("MethodKey", DisplayName = "Method scenario")]
            public void TestMethodWithScenarioAttribute()
            {
                
            }
            
            [TestScenario("ClassKey", DisplayName = "Method scenario")]
            public void TestMethodWithDuplicateClassScenarioAttribute()
            {
                
            }
        }
        
        [Fact]
        public void No_attributes_returns_empty_list()
        {
            var testMethod = XunitMocks.TestMethod(
                typeof(TestClassWithoutClassScenarioAttribute),
                nameof(TestClassWithoutClassScenarioAttribute.TestMethodWithoutScenarioAttribute));

            var scenarioAttributes = testMethod.GetScenarioAttributes();

            scenarioAttributes.Should().NotBeNull();
            scenarioAttributes.Should().BeEmpty();
        }
        
        [Fact]
        public void Attribute_on_method_returned()
        {
            var testMethod = XunitMocks.TestMethod(
                typeof(TestClassWithoutClassScenarioAttribute),
                nameof(TestClassWithoutClassScenarioAttribute.TestMethodWithScenarioAttribute));

            var scenarioAttributes = testMethod.GetScenarioAttributes();

            scenarioAttributes.Should().NotBeNull();
            scenarioAttributes.Should().HaveCount(1);
        }
        
        [Fact]
        public void Attribute_on_class_returned()
        {
            var testMethod = XunitMocks.TestMethod(
                typeof(TestClassWithClassScenarioAttribute),
                nameof(TestClassWithClassScenarioAttribute.TestMethodWithoutScenarioAttribute));

            var scenarioAttributes = testMethod.GetScenarioAttributes();

            scenarioAttributes.Should().NotBeNull();
            scenarioAttributes.Should().HaveCount(1);
        }
        
        [Fact]
        public void Attribute_on_method_and_class_returned()
        {
            var testMethod = XunitMocks.TestMethod(
                typeof(TestClassWithClassScenarioAttribute),
                nameof(TestClassWithClassScenarioAttribute.TestMethodWithScenarioAttribute));

            var scenarioAttributes = testMethod.GetScenarioAttributes();

            scenarioAttributes.Should().NotBeNull();
            scenarioAttributes.Should().HaveCount(2);
        }
        
        [Fact]
        public void Duplicate_attribute_on_method_and_class_returns_method_attribute()
        {
            var testMethod = XunitMocks.TestMethod(
                typeof(TestClassWithClassScenarioAttribute),
                nameof(TestClassWithClassScenarioAttribute.TestMethodWithDuplicateClassScenarioAttribute));

            var scenarioAttributes = testMethod.GetScenarioAttributes();

            scenarioAttributes.Should().NotBeNull();
            scenarioAttributes.Should().HaveCount(1);

            var attribute = (ScenarioAttribute) ((IReflectionAttributeInfo) scenarioAttributes.Single()).Attribute;
            attribute.DisplayName.Should().Be("Method scenario");
        }
    }
}