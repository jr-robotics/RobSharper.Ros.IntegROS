using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RosComponentTesting;
using RosComponentTesting.TestSteps;
using Xunit;
using It = Moq.It;

namespace RosComponentTestingTests.RosTestBuilderTests
{
    public class ExecuteTests
    {
        [Fact]
        public void Can_execute_without_expectation()
        {
            RegisterMockTestExecutor();

            new RosTestBuilder()
                .Execute();
        }

        [Fact]
        public void Can_execute_with_expectation()
        {
            RegisterMockTestExecutor();
            
            new RosTestBuilder()
                .Expect(new Mock<IExpectation>().Object)
                .Execute();
        }

        private static void RegisterMockTestExecutor()
        {
            var executorFactoryMock = new Mock<ITestExecutorFactory>();
            executorFactoryMock
                .Setup(m => m.Create(It.IsAny<IEnumerable<ITestStep>>(), It.IsAny<IEnumerable<IExpectation>>()))
                .Returns<IEnumerable<ITestStep>, IEnumerable<IExpectation>>((steps, expectations) =>
                {
                    var mock = new Mock<TestExecutorBase>(steps, expectations);
                    return mock.Object;
                });

            DependencyResolver.Services.AddSingleton<ITestExecutorFactory>(x => executorFactoryMock.Object);
        }
    }
}