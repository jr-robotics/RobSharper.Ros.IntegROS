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
        public enum RosTestExecutionState
        {
            NotStarted,
            Setup,
            Running,
            TearDown,
            Finished,
            Canceled
        }
        
        private readonly ICollection<ITestStep> _steps;
        private readonly IEnumerable<IExpectation> _expectations;
        
        private Task _rosShutdownTask;
        private CancellationTokenSource _cancellationTokenSource;
        private RosPublisherCollection _publisherCollection;
        private ExceptionDispatcher _exceptionDispatcher;
        private NodeHandle _nodeHandle;
        private AsyncSpinner _spinner;

        public RosTestExecutionState State { get; private set; }

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
            if (State != RosTestExecutionState.NotStarted)
            {
                throw new InvalidOperationException("Executor is not in a valid state.");
            }

            if (options == null)
            {
                options = TestExecutionOptions.Default;
            }

            try
            {
                State = RosTestExecutionState.Setup;
                
                Setup();
                await RegisterRosComponentsAsync(_nodeHandle);

                // Wait until timeout expires or cancellation requested
                _cancellationTokenSource.CancelAfter(options.Timeout);

                // Execute Steps
                State = RosTestExecutionState.Running;
                ExecuteSteps();
                
                State = RosTestExecutionState.TearDown;
                
                // No Cancellation from this point on
                _cancellationTokenSource.Dispose();
                
                // Start ROS shutdown in background
                InitiateRosShutdown();

                VerifyExecution();
            }
            finally
            {
                await ShutdownAsync();

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    State = RosTestExecutionState.Canceled;
                }
                else
                {
                    State = RosTestExecutionState.Finished;
                }
            }
        }

        private void Setup()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _exceptionDispatcher = new ExceptionDispatcher(_cancellationTokenSource);
            _publisherCollection = new RosPublisherCollection();

            RegisterCancellationCallbacks();
          
            // ROS Specific setup

            ROS.Init(new string[0], "TESTNODE");
                
            _spinner = new AsyncSpinner();
            _spinner.Start();

            _nodeHandle = new NodeHandle();
        }

        private void RegisterCancellationCallbacks()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            foreach (var expectation in _expectations)
            {
                cancellationToken.Register(expectation.Cancel);
            }
        }

        private async Task RegisterRosComponentsAsync(NodeHandle node)
        {
            var awaitableRosRegistrationTasks = new List<Task>();
            var expectations = _steps
                .OfType<IExpectationStep>()
                .Select(s => s.Expectation)
                .Union(_expectations);

            // Register Subscribers
            foreach (var expectation in expectations)
            {
                var t = RegisterSubscribersAsync(expectation, node, _exceptionDispatcher);
                awaitableRosRegistrationTasks.Add(t);
            }

            // Register Publishers
            var publicationTopics = _steps
                .OfType<PublicationStep>()
                .Select(s => s.Publication.Topic)
                .Distinct();

            foreach (var topic in publicationTopics)
            {
                var t = RegisterPublisher(topic, node, _publisherCollection);
                awaitableRosRegistrationTasks.Add(t);
            }

            await Task.WhenAll(awaitableRosRegistrationTasks).ConfigureAwait(false);
        }

        private void ExecuteSteps()
        {
            var stepExecutionFactory = new StepExecutorFactory();
            var serviceProvider = BuildServiceProvider(_publisherCollection);
            var cancellationToken = _cancellationTokenSource.Token;

            foreach (var expectation in _expectations)
            {
                expectation.Activate();
            }

            foreach (var step in _steps)
            {
                var stepExecutor = stepExecutionFactory.CreateExecutor(step);
                cancellationToken.Register(stepExecutor.Cancel);

                stepExecutor.Execute(serviceProvider);
            }

            foreach (var expectation in _expectations)
            {
                expectation.Deactivate();
            }
        }

        private void VerifyExecution()
        {
            // Check for unhandled exception
            if (_exceptionDispatcher.HasException)
            {
                throw _exceptionDispatcher.Exception;
            }
            
            // If the execution wa cancelled and exception dispatcher has no exception,
            // the cancellation was caused by a timeout
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                TestFrameworkProvider.Framework.Throw("The execution timed out");
            }

            ValidateExpectations();
        }

        private void ValidateExpectations()
        {
            var validationErrors = _expectations
                .SelectMany(e => e.GetValidationErrors())
                .ToList();

            if (validationErrors.Any())
            {
                var errorMessage = BuildErrorMessage(validationErrors);
                TestFrameworkProvider.Framework.Throw(errorMessage);
            }
        }

        private void InitiateRosShutdown()
        {
            if (_rosShutdownTask == null)
            {
                _rosShutdownTask = ROS.Shutdown();
            }
        }

        private async Task ShutdownAsync()
        {
            InitiateRosShutdown();
            await _rosShutdownTask.ConfigureAwait(false);
        }

        private IServiceProvider BuildServiceProvider(IRosPublisherResolver publisherResolver)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.Add(DependencyResolver.Services);
            
            serviceCollection.AddSingleton<IRosPublisherResolver>(publisherResolver);
            
            return serviceCollection.BuildServiceProvider();
        }

        private async Task RegisterSubscribersAsync(IExpectation expectation, NodeHandle node,
            ExceptionDispatcher exceptionDispatcher)
        {
            if (expectation is ITopicExpectation topicExpectation)
            {
                ROS.RegisterMessageAssembly(topicExpectation.TopicType.Assembly);
                
                await node.SubscribeAsync(SubscribeOptionsFactory.Create(topicExpectation, exceptionDispatcher));
            }
            else
            {
                throw new NotSupportedException($"Expectation type {expectation.GetType()} is not supported.");
            }
        }
        
        private async Task RegisterPublisher(TopicDescriptor topic, NodeHandle node,
            RosPublisherCollection publisherCollection)
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
            
            var t = Task.Run(async () =>
            {
                var rosPublisher = advertiseMethod.Invoke(node, new object[] {topic.Topic, 1});
                var publisherProxy = RosPublisherProxy.Create(topic, rosPublisher);

                // Give all subscribers in the ROS network a chance to subscribe before
                // publishing starts 
                await Task.Delay(1000).ConfigureAwait(false);
                
                publisherCollection.Add(topic, publisherProxy);
            });
            
            await t;
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