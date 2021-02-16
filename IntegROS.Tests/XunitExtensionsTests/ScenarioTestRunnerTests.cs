using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IntegROS.Tests.XunitExtensionsTests.Utility;
using IntegROS.XunitExtensions;
using IntegROS.XunitExtensions.ScenarioDiscovery;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.Tests.XunitExtensionsTests
{
    public class ScenarioTestRunnerTests
    {
        private void AssertRunSummary(RunSummary runSummary, int failed = 0, int skipped = 0, int succeeded = 0)
        {
            runSummary.Should().NotBeNull();
            
            var total = skipped + failed + succeeded;
            
            runSummary.Total.Should().Be(total);
            runSummary.Skipped.Should().Be(skipped);
            runSummary.Failed.Should().Be(failed);
        }
        
        [Fact]
        public async void Valid_scenario()
        {
            var runner = CreateRunner(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_scenario));

            var invokeCalls = 0;
            runner.OnInvokeTestMethod = () => invokeCalls++;
            
            var result = await runner.RunAsync();
            
            AssertRunSummary(result, succeeded: 1);
            invokeCalls.Should().Be(1);
        }
        
        [Fact]
        public async void Skipped_scenario()
        {
            var runner = CreateRunner(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_skipped_scenario));

            var invokeCalls = 0;
            runner.OnInvokeTestMethod = () => invokeCalls++;
            
            var result = await runner.RunAsync();
            
            AssertRunSummary(result, skipped: 1);
            invokeCalls.Should().Be(0);
        }
        
        [Fact]
        public async void No_scenario()
        {
            var runner = CreateRunner(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_null_scenario));

            var invokeCalls = 0;
            runner.OnInvokeTestMethod = () => invokeCalls++;
            
            var result = await runner.RunAsync();
            
            AssertRunSummary(result, failed: 1);
            invokeCalls.Should().Be(0);
        }
        
        
        protected ExceptionAggregator Aggregator { get; }
        protected Mock<IMessageBus> MessageBus { get; }
        
        public ScenarioTestRunnerTests()
        {
            MessageBus = new Mock<IMessageBus>();
            MessageBus.Setup(x => x.QueueMessage(It.IsAny<IMessageSinkMessage>())).Returns(true);
            
            Aggregator = new ExceptionAggregator();
        }
        
        protected ScenarioTestRunnerMock CreateRunner(Type testClass, string testMethodName)
        {
            var testCaseDiscoverer =
                new ScenarioExpectationDiscovererTests.TestableScenarioExpectationDiscoverer(new NullMessageSink())
                {
                    PreEnumerateTestCases = true
                };

            var testMethod = XunitMocks.TestMethod(testClass, testMethodName);
            var expectThatAttribute = XunitMocks.ExpectThatAttribute();
            var testCases = testCaseDiscoverer.Discover(new TestFrameworkOptions(), testMethod, expectThatAttribute);
            var testCase = testCases.SingleOrDefault() as ScenarioTestCaseBase;

            if (testCase == null)
                throw new NotSupportedException("Test case is not a ScenarioTestCaseBase");
            
            var test = new ScenarioTest(testCase, testCase.DisplayName, testCase.ScenarioIdentifier);
            
            var runner = new ScenarioTestRunnerMock(test,
                new NullMessageSink(),
                MessageBus.Object,
                test.TestCase.TestMethod.TestClass.Class.ToRuntimeType(),
                new object[0],
                test.TestCase.Method.ToRuntimeMethod(),
                new object[0],
                testCase.SkipReason,
                new BeforeAfterTestAttribute[0],
                Aggregator,
                new CancellationTokenSource()
            );

            return runner;
        }
        public class ScenarioTestRunnerMock : ScenarioTestRunner
        {
            public Action OnAfterTestStartingBegin { get; set; } = () => { };
            public Action OnAfterTestStartingEnd { get; set; } = () => { };
            public Action OnInvokeTestMethod { get; set; } = () => { };

            public ScenarioTestRunnerMock(ScenarioTest test, IMessageSink diagnosticMessageSink, IMessageBus messageBus,
                Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments,
                string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterTestAttributes,
                ExceptionAggregator exceptionAggregator, CancellationTokenSource cancellationTokenSource) : base(test,
                diagnosticMessageSink, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                skipReason, beforeAfterTestAttributes, exceptionAggregator, cancellationTokenSource)
            {
            }

            protected override void AfterTestStarting()
            {
                OnAfterTestStartingBegin();
                base.AfterTestStarting();
                OnAfterTestStartingEnd();
            }

            protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
            {
                OnInvokeTestMethod();
                return Task.Run(() => (decimal) 0);
            }

        }
    }
}