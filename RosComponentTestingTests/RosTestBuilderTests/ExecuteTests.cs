using System;
using Moq;
using RosComponentTesting;
using Xunit;

namespace RosComponentTestingTests.RosTestBuilderTests
{
    public class ExecuteTests
    {
        [Fact]
        public void Cannot_Execute_without_expectation()
        {
            var builder = new RosTestBuilder();

            Assert.Throws<InvalidOperationException>(() => builder.Execute());
        }

        [Fact]
        public void Can_execute_with_expectation()
        {
            new RosTestBuilder()
                .Expect(new Mock<IExpectation>().Object)
                .Execute();
        }
    }
}