using System;
using System.Reflection;
using System.Threading;
using Uml.Robotics.Ros;

namespace RosComponentTesting
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
            private readonly ExpectationErrorHandler _errorHandler;

            public TopicExpectationCallbackProxy(ITopicExpectation expectation,
                ExpectationErrorHandler errorHandler)
            {
                _expectation = expectation;
                _errorHandler = errorHandler;
            }
            
            // ReSharper disable once UnusedMember.Local
            public void Callback(object message)
            {
                try
                {
                    _expectation.OnReceiveMessage(message);
                }
                catch (Exception e)
                {
                    _errorHandler.AddError(new ExpectationError(_expectation, e));
                    _errorHandler.Cancel();
                }
            }
        }
        
        public static SubscribeOptions Create(ITopicExpectation expectation,
            ExpectationErrorHandler errorHandler)
        {
            var m = (RosMessage) Activator.CreateInstance(expectation.TopicType);
            var callbackProxy = new TopicExpectationCallbackProxy(expectation, errorHandler);
            

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