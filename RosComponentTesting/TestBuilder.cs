using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using RosComponentTesting.TestSteps;

namespace RosComponentTesting
{
    public class TestBuilder
    {
        private readonly ICollection<IExpectation> _expectations = new List<IExpectation>();
        private readonly ICollection<ITestStep> _steps = new List<ITestStep>();
        private ITestExecutorFactory _testExecutorFactory;

        public ITestExecutorFactory TestExecutorFactory
        {
            set => _testExecutorFactory = value;
            get
            {
                if (_testExecutorFactory == null)
                {
                    _testExecutorFactory = DependencyResolver.Services
                        .BuildServiceProvider()
                        .GetService<ITestExecutorFactory>();


                    if (_testExecutorFactory == null)
                    {
                        throw new InvalidOperationException(
                            "Cannot create test executor. No test executor factory registered!");
                    }
                }

                return _testExecutorFactory;
            }
        }

        public ITestExecutor TestExecutor => TestExecutorFactory.Create(_steps, _expectations);

        public TestBuilder Publish(string advertiseTopic, object message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            var topic = new TopicDescriptor(advertiseTopic, message.GetType());
            var publication = new Publication(topic, message);
            
            Publish(publication);
            
            return this;
        }

        public TestBuilder Publish(IPublication publication)
        {
            var step = new PublicationStep(publication);
            _steps.Add(step);
            
            return this;
        }

        public TestBuilder Expect<T>(string subscribeTopic, Func<T, bool> func, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Expect(subscribeTopic, new Match<T>(func), callerFilePath, lineNumber);
        }

        public TestBuilder Expect<T>(string subscriberTopic, Match<T> match, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Expect<T>(x => x
                .Topic(subscriberTopic)
                .Match(match), 
                callerFilePath, lineNumber);
        }

        public TestBuilder Expect<T>(Action<TopicExpectationBuilder<T>> builderAction, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var builder = new TopicExpectationBuilder<T>()
                .Occurrences(Times.AtLeast(1), callerFilePath, lineNumber);
            
            builderAction(builder);

            return Expect(builder.Expectation);
        }

        public TestBuilder Expect(IExpectation expectation)
        {
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));

            _expectations.Add(expectation);
            return this;
        }

        public TestBuilder Wait(TimeSpan duration)
        {
            var step = new WaitStep(duration);
            _steps.Add(step);

            return this;
        }
        
        public TestBuilder WaitFor<T>(Action<WaitForExpectationBuilder<T>> builderAction, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            var builder = new WaitForExpectationBuilder<T>()
                .Occurrences(Times.AtLeast(1), callerFilePath, lineNumber);

            builderAction(builder);

            var expectation = builder.Expectation;
            var step = new WaitForExpectationStep<T>(expectation);
            _steps.Add(step);
            
            return this;
        }
        
        public void Execute(TestExecutionOptions options = null)
        {
            var testExecutor = TestExecutorFactory.Create(_steps, _expectations);
            testExecutor.Execute(options);
        }
    }
}