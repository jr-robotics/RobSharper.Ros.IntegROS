using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RosComponentTesting.TestFrameworks;
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
            var errorHandler = new ExpectationErrorHandler(cancellationTokenSource, _expectations);

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
                if (errorHandler.HasErrors)
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

        private static string BuildErrorMessage(IEnumerable<string> errors)
        {
            var m = new StringBuilder();

            m.AppendLine($"{errors.Count()} Expectations not met.");
            m.AppendLine();

            foreach (var error in errors)
            {
                m.AppendLine(error);
            }

            return m.ToString();
        }
    }
}