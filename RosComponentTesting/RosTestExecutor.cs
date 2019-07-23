using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uml.Robotics.Ros;

namespace RosComponentTesting
{
    public class RosTestExecutor
    {
        private readonly IEnumerable<IExpectation> _expectations;

        public RosTestExecutor(IEnumerable<IExpectation> expectations)
        {
            if (expectations == null) throw new ArgumentNullException(nameof(expectations));

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

            // TODO: remove static master uri - make it configurable!
            ROS.ROS_MASTER_URI = "http://localhost:11311";
            
            // TODO: Initialization shoud be done somewhere else
            ROS.Init(new string[0], "TESTNODE");

            var spinner = new AsyncSpinner();
            spinner.Start();

            var node = new NodeHandle();
            var awaitableRosRegistrationTasks = new List<Task>();
            
            
            var cancellationTokenSource = new CancellationTokenSource();
            var errorHandler = new ExpectationErrorHandler(cancellationTokenSource);

            // Register Subscribers
            foreach (var expectation in _expectations)
            {
                var t = RegisterSubscribers(expectation, node, errorHandler);
                awaitableRosRegistrationTasks.Add(t);
            }
            
            // TODO Register Publishers


            foreach (var task in awaitableRosRegistrationTasks)
            {
                await task;
            }

            foreach (var expectation in _expectations)
            {
                expectation.Activate();
            }
            
            // TODO Publish Messages

            
            
            
            // Wait until timeout expires or cancellation requested
            cancellationTokenSource.Token.WaitHandle.WaitOne(options.Timeout);
            
            foreach (var expectation in _expectations)
            {
                expectation.Deactivate();
            }
            
            var shutdownTask = ROS.Shutdown();

            try
            {
                // Check for unhandled exceptions
                if (errorHandler.Errors.Any())
                {
                    var innerExceptions = errorHandler.Errors.Select(e => e.Exception).ToList();
                    throw new AggregateException("Execution was canceled", innerExceptions);
                }
                
                // Check expectation validations 
                var validationErrors = _expectations.SelectMany(e => e.GetValidationErrors());

                if (validationErrors.Any())
                {
                    throw new ValidationException(validationErrors);
                }
            }
            finally
            {
                await shutdownTask;
            }
        }

        private Task RegisterSubscribers(IExpectation expectation, NodeHandle node,
            ExpectationErrorHandler errorHandler)
        {
            Task t = null;
            
            if (expectation is ITopicExpectation topicExpectation)
            {
                ROS.RegisterMessageAssembly(topicExpectation.TopicType.Assembly);
                
                t = node.SubscribeAsync(SubscribeOptionsFactory.Create(topicExpectation, errorHandler));
            }

            if (t == null)
            {
                throw new NotSupportedException($"Expectation type {expectation.GetType()} is not supported.");
            }
            
            return t;
        }
    }

    public class ValidationException : Exception
    {
        private readonly IEnumerable<string> _errors;

        public ValidationException(IEnumerable<string> errors)
        {
            _errors = errors;
        }

        public override string Message
        {
            get
            {
                var m = new StringBuilder();
                
                m.AppendLine("Expectations not met:");

                foreach (var error in _errors)
                {
                    m.AppendLine(error);
                }

                return m.ToString();
            }
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }
    }

    public class ExpectationErrorHandler
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<ExpectationError> _errors;

        public IEnumerable<ExpectationError> Errors => _errors.AsReadOnly();

        public ExpectationErrorHandler(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _errors = new List<ExpectationError>();
        }

        public void AddError(ExpectationError error)
        {
            _errors.Add(error);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }

    public class ExpectationError
    {
        public IExpectation Expectation { get; }
        public Exception Exception { get; }

        public ExpectationError(IExpectation expectation, Exception exception)
        {
            Expectation = expectation;
            Exception = exception;
        }
    }
}