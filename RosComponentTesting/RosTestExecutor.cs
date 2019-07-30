using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RosComponentTesting.ExpectationProcessing;
using RosComponentTesting.TestFrameworks;
using RosComponentTesting.TestSteps;
using Uml.Robotics.Ros;

namespace RosComponentTesting
{
    public class RosTestExecutor
    {
        private readonly ICollection<ITestStep> _steps;
        private readonly IEnumerable<IExpectation> _expectations;

        public RosTestExecutor(ICollection<ITestStep> steps, IEnumerable<IExpectation> expectations)
        {
            if (steps == null) throw new ArgumentNullException(nameof(steps));
            if (expectations == null) throw new ArgumentNullException(nameof(expectations));

            _steps = steps;
            _expectations = expectations;
        }

        public void Execute(TestExecutionOptions options = null)
        {
            var t = ExecuteAsync(options);
            t.GetAwaiter().GetResult();
        }

        public async Task ExecuteAsync(TestExecutionOptions options = null)
        {
            if (options == null)
            {
                options = TestExecutionOptions.Default;
            }

            var spinner = new AsyncSpinner();
            spinner.Start();

            var node = new NodeHandle();
            var awaitableRosRegistrationTasks = new List<Task>();
            
            var cancellationTokenSource = new CancellationTokenSource();
            var errorHandler = new ExceptionDispatcher(cancellationTokenSource);

            var publisherCollection = new RosPublisherCollection();
            var stepExecutionFactory = new StepExecutorFactory();
            var serviceProvider = BuildServiceProvider(publisherCollection);

            Task rosShutdownTask = null;
            
            try
            {
                // Register Subscribers
                foreach (var expectation in _expectations)
                {
                    var t = RegisterSubscribers(expectation, node, errorHandler);
                    awaitableRosRegistrationTasks.Add(t);
                }

                // Register Publishers
                var publicationTopics = _steps
                    .OfType<PublicationStep>()
                    .Select(s => s.Publication.Topic)
                    .Distinct();
            
                foreach (var topic in publicationTopics)
                {
                    var t = RegisterPublisher(topic, node, publisherCollection, errorHandler);
                    awaitableRosRegistrationTasks.Add(t);
                }
            
                foreach (var task in awaitableRosRegistrationTasks)
                {
                    await task;
                }
            
                cancellationTokenSource.CancelAfter(options.Timeout);
            
                foreach (var expectation in _expectations)
                {
                    expectation.Activate();
                }
            
                // Execute Steps
                foreach (var step in _steps)
                {
                    await Task.Run(() =>
                    {
                        var stepExecutor = stepExecutionFactory.CreateExecutor(step);
                        stepExecutor.Execute(serviceProvider, cancellationTokenSource);
                    }, cancellationTokenSource.Token);
                }
            
                // Wait until timeout expires or cancellation requested
                cancellationTokenSource.Token.WaitHandle.WaitOne(options.Timeout);
            
                foreach (var expectation in _expectations)
                {
                    expectation.Deactivate();
                }
            
                rosShutdownTask = ROS.Shutdown();

                // Check for unhandled exception
                if (errorHandler.HasError)
                {
                    errorHandler.Throw();
                }
                
                // Check expectation validations 
                var validationErrors = _expectations
                    .SelectMany(e => e.GetValidationErrors())
                    .ToList();

                if (validationErrors.Any())
                {
                    var errorMessage = BuildErrorMessage(validationErrors);
                    TestFrameworkProvider.Framework.Throw(errorMessage);
                }
            }
            finally
            {
                if (rosShutdownTask == null)
                {
                    rosShutdownTask = ROS.Shutdown();
                }
                
                await rosShutdownTask;
            }
        }

        private IServiceProvider BuildServiceProvider(IRosPublisherResolver publisherResolver)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.Add(DependencyResolver.Services);
            
            serviceCollection.AddSingleton<IRosPublisherResolver>(publisherResolver);
            
            return serviceCollection.BuildServiceProvider();
        }

        private Task RegisterSubscribers(IExpectation expectation, NodeHandle node,
            ExceptionDispatcher exceptionDispatcher)
        {
            Task t = null;
            
            if (expectation is ITopicExpectation topicExpectation)
            {
                ROS.RegisterMessageAssembly(topicExpectation.TopicType.Assembly);
                
                t = node.SubscribeAsync(SubscribeOptionsFactory.Create(topicExpectation, exceptionDispatcher));
            }

            if (t == null)
            {
                throw new NotSupportedException($"Expectation type {expectation.GetType()} is not supported.");
            }
            
            return t;
        }
        
        private Task RegisterPublisher(TopicDescriptor topic, NodeHandle node,
            RosPublisherCollection publisherCollection, ExceptionDispatcher exceptionDispatcher)
        {
            var messageType = topic.Type;

            var advertiseMethod = node
                .GetType()
                .GetMethod("Advertise", new[] {typeof(string), typeof(int)})
                ?.MakeGenericMethod(messageType);
            
            if (advertiseMethod == null)
            {
                throw new NotSupportedException("Could not retrieve AdvertiseAsync method");
            }
            
            var t = Task.Run(() =>
            {
                var rosPublisher = advertiseMethod.Invoke(node, new object[] {topic.Topic, 1});
                var publisherProxy = RosPublisherProxy.Create(topic, rosPublisher);

                // Give all subscribers in the ROS network a chance to subscribe before
                // publishing starts 
                Thread.Sleep(1000);
                
                publisherCollection.Add(topic, publisherProxy);
            });
            
            return t;
        }

        private static string BuildErrorMessage(List<ValidationError> errors)
        {
            var m = new StringBuilder();

            m.AppendLine($"{errors.Count()} Expectations not met.");
            m.AppendLine();

            foreach (var error in errors)
            {
                m.AppendLine(error.ToString());
            }

            return m.ToString();
        }
    }
}