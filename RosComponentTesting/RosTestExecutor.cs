using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RosComponentTesting.ExpectationProcessing;
using RosComponentTesting.TestFrameworks;
using Uml.Robotics.Ros;

namespace RosComponentTesting
{
    public class RosTestExecutor
    {
        private readonly IEnumerable<IExpectation> _expectations;
        private readonly PublicationCollection _publications;
        
        private readonly Dictionary<TopicDescriptor, IRosPublisher> _rosPublishers = new Dictionary<TopicDescriptor, IRosPublisher>();

        public RosTestExecutor(IEnumerable<IExpectation> expectations, PublicationCollection publications)
        {
            if (expectations == null) throw new ArgumentNullException(nameof(expectations));
            if (publications == null) throw new ArgumentNullException(nameof(publications));

            _expectations = expectations;
            _publications = publications;
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
            var errorHandler = new ExpectationErrorHandler(cancellationTokenSource, _expectations);

            // Register Subscribers
            foreach (var expectation in _expectations)
            {
                var t = RegisterSubscribers(expectation, node, errorHandler);
                awaitableRosRegistrationTasks.Add(t);
            }

            // Register Publishers
            foreach (var topic in _publications.Topics)
            {
                var t = RegisterPublisher(topic, node, errorHandler);
                awaitableRosRegistrationTasks.Add(t);
            }
            
            foreach (var task in awaitableRosRegistrationTasks)
            {
                await task;
            }

            foreach (var expectation in _expectations)
            {
                expectation.Activate();
            }
            

            // Publish Messages
            foreach (var publication in _publications)
            {
                _rosPublishers[publication.Topic].Publish(publication.Message);
            }
            
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
        
        private Task RegisterPublisher(TopicDescriptor topic, NodeHandle node, ExpectationErrorHandler errorHandler)
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
                
                lock (_rosPublishers)
                {
                    _rosPublishers.Add(topic, publisherProxy);
                }
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

    internal class RosPublisherProxy : IRosPublisher
    {

        public static RosPublisherProxy Create(TopicDescriptor topic, object rosPublisher)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (rosPublisher == null) throw new ArgumentNullException(nameof(rosPublisher));
            
            var publishMethod = typeof(Publisher<>)
                .MakeGenericType(topic.Type)
                .GetMethod("Publish");

            return new RosPublisherProxy(topic, publishMethod, rosPublisher);
        }


        private readonly MethodInfo _publishMethod;
        private readonly object _rosPublisher;

        public TopicDescriptor Topic { get; }

        private RosPublisherProxy(TopicDescriptor topic, MethodInfo publishMethod, object rosPublisher)
        {
            Topic = topic;
            _publishMethod = publishMethod;
            _rosPublisher = rosPublisher;
        }

        public void Publish(object msg)
        {
            _publishMethod.Invoke(_rosPublisher, new[] {msg});
        }
    }

    internal interface IRosPublisher
    {
        void Publish(object msg);
    }
}