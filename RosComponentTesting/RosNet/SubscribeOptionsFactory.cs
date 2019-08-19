using System;
using System.Reflection;
using Uml.Robotics.Ros;

namespace RosComponentTesting.RosNet
{
    internal static class SubscribeOptionsFactory
    {
        private class TopicExpectationCallbackProxy
        {
            private static MethodInfo _callbackMethod;
            public static MethodInfo CallBackMethod
            {
                get
                {
                    if (_callbackMethod == null)
                    {
                        _callbackMethod = typeof(TopicExpectationCallbackProxy).GetMethod("Callback");
                    }
                    
                    return _callbackMethod;
                }
            }
            
            
            private readonly ITopicExpectation _expectation;
            private readonly ExceptionDispatcher _exceptionDispatcher;

            public TopicExpectationCallbackProxy(ITopicExpectation expectation,
                ExceptionDispatcher exceptionDispatcher)
            {
                _expectation = expectation;
                _exceptionDispatcher = exceptionDispatcher;
            }
            
            // ReSharper disable once UnusedMember.Local
            public void Callback(object message)
            {
                try
                {
                    _expectation.HandleMessage(message);
                }
                catch (Exception e)
                {
                    // ROS has implemented the try/catch/ignore pattern.
                    // We have to intercept the callback and forward the exception to someone who 
                    // is caring about it
                    _exceptionDispatcher.Dispatch(e);
                }
            }
        }
        
        public static SubscribeOptions Create(ITopicExpectation expectation,
            ExceptionDispatcher exceptionDispatcher)
        {
            var m = (RosMessage) Activator.CreateInstance(expectation.TopicType);
            var callbackProxy = new TopicExpectationCallbackProxy(expectation, exceptionDispatcher);
            

            var callbackHelperType = typeof(SubscriptionCallbackHelper<>).MakeGenericType(new[] {expectation.TopicType});

            var callbackDelegateType = typeof(CallbackDelegate<>).MakeGenericType(new[] {expectation.TopicType});
            var callbackDelegate = Delegate.CreateDelegate(callbackDelegateType, callbackProxy,
                TopicExpectationCallbackProxy.CallBackMethod);
            
            var callbackHelper = Activator.CreateInstance(callbackHelperType, new object[] { m.MessageType, callbackDelegate }) as ISubscriptionCallbackHelper;
            
            
            var options = new SubscribeOptions(expectation.TopicName, m.MessageType, m.MD5Sum(), 1, callbackHelper);
            return options;
        }
    }
}