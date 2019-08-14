using FluentAssertions;
using RosComponentTesting.TestFrameworks;
using Xunit;
using Xunit.Sdk;

namespace RosComponentTestingTests.TestFrameworksTests
{
    public class Xunit2TestFrameworkTests
    {
        [Fact]
        public void IsAvailable_returns_true_if_xunit2_assembly_is_available()
        {
            var target = new Xunit2TestFramework();

            // xunit should be loaded in an xunit unit test.
            target.IsAvailable.Should().BeTrue();
        }
        
        [Fact]
        public void Throw_throws_xunit_exception()
        {
            const string errorMessage = "This is the error";
            var target = new Xunit2TestFramework();

            target
                .Invoking(t => t.Throw(errorMessage))
                .Should()
                .Throw<XunitException>()
                .WithMessage(errorMessage);
        }
        
        [Fact]
        public void Throw_without_message_throws_xunit_exception()
        {
            const string errorMessage = null;
            var target = new Xunit2TestFramework();

            target
                .Invoking(t => t.Throw(errorMessage))
                .Should()
                .Throw<XunitException>();
        }
    }
}