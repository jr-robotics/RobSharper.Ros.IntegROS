﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    public class ExpectationTestRunner : TestRunner<ExpectationTestCase>
    {
        readonly Specification specification;
        readonly ExecutionTimer timer;
        
        public ExpectationTestRunner(Specification specification,
                                     ITest test,
                                     IMessageBus messageBus,
                                     ExecutionTimer timer,
                                     Type testClass,
                                     MethodInfo testMethod,
                                     ExceptionAggregator aggregator,
                                     CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, null, testMethod, null, null, aggregator, cancellationTokenSource)
        {
            this.specification = specification;
            this.timer = timer;
        }

        protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
        {
            var duration = await new ExpectationTestInvoker(specification, Test, MessageBus, TestClass, TestMethod, aggregator, CancellationTokenSource).RunAsync();
            return Tuple.Create(duration, String.Empty);
        }
    }
}

