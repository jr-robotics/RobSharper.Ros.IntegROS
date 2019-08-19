using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RosComponentTesting.MessageHandling;
using RosComponentTesting.TestFrameworks;
using RosComponentTesting.TestSteps;

namespace RosComponentTesting
{
    public abstract class TestExecutorBase : ITestExecutor
    {
        private readonly IEnumerable<ITestStep> _steps;
        private readonly IEnumerable<IExpectation> _expectations;
        
        private CancellationTokenSource _cancellationTokenSource;
        private RosPublisherCollection _publisherCollection;
        private ExceptionDispatcher _exceptionDispatcher;

        public TestExecutionState State { get; private set; }

        public TestExecutorBase(IEnumerable<ITestStep> steps, IEnumerable<IExpectation> expectations)
        {
            if (steps == null) throw new ArgumentNullException(nameof(steps));
            if (expectations == null) throw new ArgumentNullException(nameof(expectations));

            _steps = steps;
            _expectations = expectations;
        }

        public async Task ExecuteAsync(TestExecutionOptions options = null)
        {
            if (State != TestExecutionState.NotStarted)
            {
                throw new InvalidOperationException("Executor is not in a valid state.");
            }

            if (options == null)
            {
                options = TestExecutionOptions.Default;
            }

            try
            {
                State = TestExecutionState.Setup;
                
                SetupInternal();
                await RegisterRosComponentsAsync();

                // Wait until timeout expires or cancellation requested
                _cancellationTokenSource.CancelAfter(options.Timeout);

                // Execute Steps
                State = TestExecutionState.Running;
                ExecuteSteps();
                
                State = TestExecutionState.TearDown;
                
                // No Cancellation from this point on
                _cancellationTokenSource.Dispose();

                VerifyExecution();
            }
            finally
            {
                await ShutdownAsync();
                
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    State = TestExecutionState.Canceled;
                }
                else
                {
                    State = TestExecutionState.Finished;
                }
            }
        }

        protected abstract void Setup();

        protected abstract Task RegisterSubscriberAsync(ITopicExpectation topicExpectation,
            ExceptionDispatcher exceptionDispatcher);

        protected abstract Task RegisterPublisherAsync(TopicDescriptor topic,
            RosPublisherCollection publisherCollection);

        protected abstract Task ShutdownAsync();
        
        private void SetupInternal()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _exceptionDispatcher = new ExceptionDispatcher(_cancellationTokenSource);
            _publisherCollection = new RosPublisherCollection();

            RegisterCancellationCallbacks();
            
            Setup();
        }

        private void RegisterCancellationCallbacks()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            foreach (var expectation in _expectations)
            {
                cancellationToken.Register(expectation.Cancel);
            }
        }

        private async Task RegisterRosComponentsAsync()
        {
            var awaitableRosRegistrationTasks = new List<Task>();

            // Register Subscribers            
            var expectations = _steps
                .OfType<IExpectationStep>()
                .Select(s => s.Expectation)
                .Union(_expectations);

            foreach (var expectation in expectations)
            {
                if (expectation is ITopicExpectation topicExpectation)
                {
                    var t = RegisterSubscriberAsync(topicExpectation, _exceptionDispatcher);
                    awaitableRosRegistrationTasks.Add(t);
                }
            }

            // Register Publishers
            var publicationTopics = _steps
                .OfType<PublicationStep>()
                .Select(s => s.Publication.Topic)
                .Distinct();

            foreach (var topic in publicationTopics)
            {
                var t = RegisterPublisherAsync(topic, _publisherCollection);
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

        private IServiceProvider BuildServiceProvider(IRosPublisherResolver publisherResolver)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.Add(DependencyResolver.Services);
            
            serviceCollection.AddSingleton<IRosPublisherResolver>(publisherResolver);
            
            return serviceCollection.BuildServiceProvider();
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