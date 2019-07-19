using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            t.Wait();
        }

        public async Task ExecuteAsync(TestExecutionOptions options = null)
        {
            if (options == null)
            {
                options = TestExecutionOptions.Default;
            }

            // TODO: remove static master uri
            ROS.ROS_MASTER_URI = "http://localhost:11311";
            ROS.Init(new string[0], "TESTNODE");

            var spinner = new AsyncSpinner();
            spinner.Start();

            var node = new NodeHandle();
            var awaitableRosRegistrationTasks = new List<Task>();
            
            // Register Subscribers
            foreach (var expectation in _expectations)
            {
                var t = RegisterSubscribers(expectation, node);
                awaitableRosRegistrationTasks.Add(t);
            }
            
            // TODO Register Publishers


            foreach (var task in awaitableRosRegistrationTasks)
            {
                await task;
            }
            
            // TODO Publish Messages


            // TODO: Better timeout handling
            await Task.Run(() =>
            {
                Thread.Sleep(options.Timeout);
                ROS.Shutdown();
            });
        }

        private Task RegisterSubscribers(IExpectation expectation, NodeHandle node)
        {
            Task t = null;

            if (expectation is ITopicExpectation topicExpectation)
            {
                t = node.SubscribeAsync(SubscribeOptionsFactory.Create(topicExpectation));
            }

            if (t == null)
            {
                throw new NotSupportedException();
            }

            
            return t;
        }
    }

    public static class SubscribeOptionsFactory
    {
        private class TopicExpectationCallbacProxy
        {
            private static MethodInfo _callbackMethod;
            public static MethodInfo CallBackMethod
            {
                get
                {
                    if (_callbackMethod == null)
                    {
                        _callbackMethod = typeof(TopicExpectationCallbacProxy).GetMethod("Callback");
                    }
                    
                    return _callbackMethod;
                }
            }
            
            
            private readonly ITopicExpectation _expectation;

            public TopicExpectationCallbacProxy(ITopicExpectation expectation)
            {
                _expectation = expectation;
            }
            
            public void Callback(object message)
            {
                _expectation.OnReceiveMessage(message);
            }
        }
        
        public static SubscribeOptions Create(ITopicExpectation expectation)
        {
            var m = (RosMessage) Activator.CreateInstance(expectation.TopicType);
            var callbackProxy = new TopicExpectationCallbacProxy(expectation);
            

            var callbackHelperType = typeof(SubscriptionCallbackHelper<>).MakeGenericType(new[] {expectation.TopicType});

            var callbackDelegateType = typeof(CallbackDelegate<>).MakeGenericType(new[] {expectation.TopicType});
            var callbackDelegate = Delegate.CreateDelegate(callbackDelegateType, callbackProxy,
                TopicExpectationCallbacProxy.CallBackMethod);
            
            var callbackHelper = Activator.CreateInstance(callbackHelperType, new object[] { m.MessageType, callbackDelegate }) as ISubscriptionCallbackHelper;
            
            
            var options = new SubscribeOptions(expectation.TopicName, m.MessageType, m.MD5Sum(), 1, callbackHelper);
            return options;
        }
    }
}