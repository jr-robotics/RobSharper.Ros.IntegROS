using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IntegROS.Tests.XunitExtensionsTests.Utility;
using IntegROS.XunitExtensions;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.Tests.XunitExtensionsTests
{
    public class MultipleScenariosTestCaseRunnerTests
    {
        [Fact]
        public async void Method_with_one_scenario()
        {
            var runner = CreateRunner(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_scenario));
            
            var result = await runner.RunAsync();
            
            runner.TestRunners.Count().Should().Be(1);
            runner.ScenarioDiscoveryException.Should().BeNull();
            AssertRunSummary(result, succeeded: 1);
        }
            
        [Fact]
        public async void Method_with_two_scenario()
        {
            var runner = CreateRunner(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_two_different_scenarios));
            
            var result = await runner.RunAsync();
            
            runner.TestRunners.Count().Should().Be(2);
            runner.ScenarioDiscoveryException.Should().BeNull();
            AssertRunSummary(result, succeeded: 2);
        }
            
        [Fact]
        public async void Method_with_skipped_scenario()
        {
            var runner = CreateRunner(typeof(XunitExtensionsTestCases),
                nameof(XunitExtensionsTestCases.Method_with_one_skipped_scenario));
            
            var result = await runner.RunAsync();
            
            runner.TestRunners.Count().Should().Be(1);
            runner.ScenarioDiscoveryException.Should().BeNull();
            AssertRunSummary(result, skipped: 1);
        }
        
        
        protected ExceptionAggregator Aggregator { get; }
        protected Mock<IMessageBus> MessageBus { get; }
        
        public MultipleScenariosTestCaseRunnerTests()
        {
            MessageBus = new Mock<IMessageBus>();
            MessageBus.Setup(x => x.QueueMessage(It.IsAny<IMessageSinkMessage>())).Returns(true);
        
            Aggregator = new ExceptionAggregator();
        }
        
        protected MultipleScenariosTestCaseRunnerMock CreateRunner(Type testClass, string testMethodName)
        {
            return MultipleScenariosTestCaseRunnerMock.Create(testClass, testMethodName, Aggregator, MessageBus.Object);
        }
        
        protected void AssertRunSummary(RunSummary runSummary, int failed = 0, int skipped = 0, int succeeded = 0)
        {
            runSummary.Should().NotBeNull();
        
            var total = skipped + failed + succeeded;
        
            runSummary.Total.Should().Be(total);
            runSummary.Skipped.Should().Be(skipped);
            runSummary.Failed.Should().Be(failed);
        }
        
        public class MultipleScenariosTestCaseRunnerMock : MultipleScenariosTestCaseRunner
        {
            public Action OnAfterTestCaseStartingBegin { get; set; } = () => { };
            public Action OnAfterTestCaseStartingEnd { get; set; } = () => { };
            
            public new IEnumerable<XunitTestRunner> TestRunners => base.TestRunners;

            public new Exception ScenarioDiscoveryException => base.ScenarioDiscoveryException;

            public MultipleScenariosTestCaseRunnerMock(MultipleScenariosTestCase testCase, string displayName,
                string skipReason, object[] constructorArguments, IMessageSink diagnosticMessageSink,
                IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
                : base(testCase, displayName, skipReason, constructorArguments, diagnosticMessageSink, messageBus,
                    aggregator, cancellationTokenSource)
            {
            }

            protected override async Task AfterTestCaseStartingAsync()
            {
                OnAfterTestCaseStartingBegin();
                await base.AfterTestCaseStartingAsync();
                OnAfterTestCaseStartingEnd();
            }
            
            public static MultipleScenariosTestCaseRunnerMock Create(Type testClass, string testMethodName, ExceptionAggregator aggregator = null, IMessageBus bus = null)
            {
                var testCaseDiscoverer =
                    new ScenarioExpectationDiscovererTests.TestableScenarioExpectationDiscoverer(new NullMessageSink())
                    {
                        PreEnumerateTestCases = false
                    };

                var testMethod = XunitMocks.TestMethod(testClass, testMethodName);
                var expectThatAttribute = XunitMocks.ExpectThatAttribute();
                var testCases = testCaseDiscoverer.Discover(new TestFrameworkOptions(), testMethod, expectThatAttribute);
                var testCase = testCases.SingleOrDefault() as MultipleScenariosTestCase;

                if (testCase == null)
                    throw new NotSupportedException("Test case is not a MultipleScenariosTestCase");
            
                var runner = new MultipleScenariosTestCaseRunnerMock(testCase,
                    testCase.DisplayName,
                    testCase.SkipReason,
                    new object[0], 
                    new NullMessageSink(),
                    bus,
                    aggregator,
                    new CancellationTokenSource()
                );

                return runner;
            }
        }
    }
}