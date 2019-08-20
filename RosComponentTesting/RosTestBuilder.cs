using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using RosComponentTesting.TestSteps;

namespace RosComponentTesting
{
    public class RosTestBuilder
    {
        public ITestExecutorFactory TestExecutorFactory { get; set; }
        
        private readonly ICollection<IExpectation> _expectations = new List<IExpectation>();
        private readonly ICollection<ITestStep> _steps = new List<ITestStep>();

        public RosTestBuilder Publish(string advertiseTopic, object message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            var topic = new TopicDescriptor(advertiseTopic, message.GetType());
            var publication = new Publication(topic, message);
            
            Publish(publication);
            
            return this;
        }

        public RosTestBuilder Publish(IPublication publication)
        {
            var step = new PublicationStep(publication);
            _steps.Add(step);
            
            return this;
        }

        public RosTestBuilder Expect<T>(string subscribeTopic, Func<T, bool> func, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Expect(subscribeTopic, new Match<T>(func), callerFilePath, lineNumber);
        }

        public RosTestBuilder Expect<T>(string subscriberTopic, Match<T> match, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Expect<T>(x => x
                .Topic(subscriberTopic)
                .Match(match), 
                callerFilePath, lineNumber);
        }

        public RosTestBuilder Expect<T>(Action<TopicExpectationBuilder<T>> builderAction, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var builder = new TopicExpectationBuilder<T>()
                .Occurrences(Times.AtLeast(1), callerFilePath, lineNumber);
            
            builderAction(builder);

            return Expect(builder.Expectation);
        }

        public RosTestBuilder Expect(IExpectation expectation)
        {
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));

            _expectations.Add(expectation);
            return this;
        }

        public RosTestBuilder Wait(TimeSpan duration)
        {
            var step = new WaitStep(duration);
            _steps.Add(step);

            return this;
        }
        
        public RosTestBuilder WaitFor<T>(Action<WaitForExpectationBuilder<T>> builderAction, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var builder = new WaitForExpectationBuilder<T>()
                .Occurrences(Times.AtLeast(1), callerFilePath, lineNumber);

            builderAction(builder);

            var expectation = builder.Expectation;
            var step = new WaitForExpectationStep<T>(expectation);
            _steps.Add(step);
            
            return this;
        }

        public ITestExecutor ToTestExecutor()
        {
            var testExecutorFactory = TestExecutorFactory;

            if (testExecutorFactory == null)
            {
                testExecutorFactory = DependencyResolver.Services
                    .BuildServiceProvider()
                    .GetService<ITestExecutorFactory>();
                
                
                if (testExecutorFactory == null)
                {
                    throw new InvalidOperationException("Cannot create test executor. No test executor factory registered!");
                }
            }
            
            return testExecutorFactory.Create(_steps, _expectations);
        }
        
        public void Execute(TestExecutionOptions options = null)
        {
            ToTestExecutor().Execute(options);
        }
    }
}