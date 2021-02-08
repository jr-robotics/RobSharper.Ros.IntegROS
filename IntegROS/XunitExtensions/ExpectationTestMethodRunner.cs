﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTestMethodRunner : TestMethodRunner<ExpectationTestCase>
    {
        readonly Specification specification;

        public ExpectationTestMethodRunner(Specification specification,
                                           ITestMethod testMethod,
                                           IReflectionTypeInfo @class,
                                           IReflectionMethodInfo method,
                                           IEnumerable<ExpectationTestCase> testCases,
                                           IMessageBus messageBus,
                                           ExceptionAggregator aggregator,
                                           CancellationTokenSource cancellationTokenSource)
            : base(testMethod, @class, method, testCases, messageBus, aggregator, cancellationTokenSource)
        {
            this.specification = specification;
        }

        protected override Task<RunSummary> RunTestCaseAsync(ExpectationTestCase testCase)
        {
            return testCase.RunAsync(specification, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource);
        }
    }
}
