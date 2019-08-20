using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RosComponentTesting.TestSteps;
using Uml.Robotics.Ros;

namespace RosComponentTesting.RosNet
{
    public class RosTestExecutor : TestExecutorBase
    {
        public const string DEFAULT_ROS_MASTER_URI = "http://localhost:11311";
        
        private readonly string _rosMasterUri;
        private readonly string _nodeName;
        
        private NodeHandle _nodeHandle;
        private AsyncSpinner _spinner;
        
        public RosTestExecutor(IEnumerable<ITestStep> steps, IEnumerable<IExpectation> expectations) : base(steps, expectations)
        {
            _rosMasterUri = DEFAULT_ROS_MASTER_URI;
            _nodeName = CreateRandomNodeName();
        }

        public RosTestExecutor(string rosMasterUri, string nodeName, IEnumerable<ITestStep> steps, IEnumerable<IExpectation> expectations) : base(steps, expectations)
        {
            _rosMasterUri = rosMasterUri ?? DEFAULT_ROS_MASTER_URI;
            _nodeName = nodeName ?? CreateRandomNodeName();
        }

        private static string CreateRandomNodeName()
        {
            return $"RosComponentTesting_{Guid.NewGuid()}";
        }

        protected override void Setup()
        {
            // ROS Specific setup
            ROS.ROS_MASTER_URI = _rosMasterUri;
            ROS.Init(new string[0], _nodeName);

            _spinner = new AsyncSpinner();
            _spinner.Start();

            _nodeHandle = new NodeHandle();
        }

        protected override Task RegisterSubscriberAsync(ITopicExpectation topicExpectation,
            ExceptionDispatcher exceptionDispatcher)
        {
            ROS.RegisterMessageAssembly(topicExpectation.TopicType.Assembly);
            return _nodeHandle.SubscribeAsync(SubscribeOptionsFactory.Create(topicExpectation, exceptionDispatcher));
        }

        protected override Task RegisterPublisherAsync(TopicDescriptor topic, RosPublisherCollection publisherCollection)
        {
            var messageType = topic.Type;

            var advertiseMethod = _nodeHandle
                .GetType()
                .GetMethod("Advertise", new[] {typeof(string), typeof(int)})
                ?.MakeGenericMethod(messageType);
            
            if (advertiseMethod == null)
            {
                throw new NotSupportedException("Could not retrieve AdvertiseAsync method");
            }
            
            var t = Task.Run(async () =>
            {
                var rosPublisher = advertiseMethod.Invoke(_nodeHandle, new object[] {topic.Topic, 1});
                var publisherProxy = RosPublisherProxy.Create(topic, rosPublisher);

                // Give all subscribers in the ROS network a chance to subscribe before
                // publishing starts 
                await Task.Delay(1000).ConfigureAwait(false);
                
                publisherCollection.Add(topic, publisherProxy);
            });

            return t;
        }

        protected override Task ShutdownAsync()
        {
            var shutdownTask = ROS.Shutdown();

            // ROS.Shutdown() may return null
            if (shutdownTask == null)
            {
                shutdownTask = Task.CompletedTask;
            }

            return shutdownTask;
        }
    }
}