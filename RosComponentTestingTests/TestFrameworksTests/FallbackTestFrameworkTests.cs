using FluentAssertions;
using RosComponentTesting;
using RosComponentTesting.TestFrameworks;
using Xunit;

namespace RosComponentTestingTests.TestFrameworksTests
{
    public class FallbackTestFrameworkTests
    {
        [Fact]
        public void IsAvailable_returns_true()
        {
            var target = new FallbackTestFramework();

            // Fallback framework should always be available
            target.IsAvailable.Should().BeTrue();
        }
        
        [Fact]
        public void Throw_throws_exception()
        {
            const string errorMessage = "This is the error message";
            var target = new FallbackTestFramework();

            target
                .Invoking(x => x.Throw(errorMessage))
                .Should()
                .Throw<ValidationException>()
                .WithMessage(errorMessage);
        }
        
        [Fact]
        public void Throw_throws_exception_without_message()
        {
            var target = new FallbackTestFramework();

            target
                .Invoking(x => x.Throw(null))
                .Should()
                .Throw<ValidationException>();
        }
    }
}