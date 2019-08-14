using FluentAssertions;
using RosComponentTesting.TestFrameworks;
using Xunit;

namespace RosComponentTestingTests.TestFrameworksTests
{
    public class TestFrameworkProviderTests
    {
        [Fact]
        public void TestframeworkProvider_returns_a_framework()
        {
            TestFrameworkProvider.Framework.Should().NotBeNull();
        }
        
        [Fact]
        public void TestframeworkProvider_returns_a_xunit_framework()
        {
            TestFrameworkProvider.Framework.Should().BeOfType<Xunit2TestFramework>();
        }
    }
}