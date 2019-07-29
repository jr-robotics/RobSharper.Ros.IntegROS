using System;
using System.Collections.Generic;
using System.Linq;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class TopicExpectation<TTopic> : ITopicExpectation
    {
        public enum StateValue
        {
            Inactive,
            Active
        };
        
        public string TopicName { get; set; }
        
        public Type TopicType { get; set; }
        
        public StateValue State { get; private set; }

        private readonly List<ExpectationMessageHandler<TTopic>> _handlers = new List<ExpectationMessageHandler<TTopic>>();

        public void ThrowIfNotInitialized()
        {
            if (string.IsNullOrEmpty(TopicName)) throw new InvalidOperationException("Topic name not set");
            if (TopicType == null) throw new InvalidOperationException("Topic type not set");
        }

        public virtual void Activate()
        {
            if (State == StateValue.Active) return;
            
            lock (_handlers)
            {
                if (State == StateValue.Active) return;
                
                foreach (var handler in _handlers)
                {
                    handler.OnActivateExpectation();
                }

                State = StateValue.Active;
            }
        }

        public virtual void Deactivate()
        {
            if (State == StateValue.Inactive) return;

            lock (_handlers)
            {
                if (State == StateValue.Inactive) return;
                
                foreach (var handler in _handlers)
                {
                    handler.OnDeactivateExpectation();
                }

                State = StateValue.Inactive;
            }
        }

        public void OnReceiveMessage(object message)
        {
            if (State != StateValue.Active) return;
            
            HandleMessage((TTopic) message);;
        }
        
        protected virtual void HandleMessage(TTopic message)
        {
            var context = new ExpectationRuleContext();
            
            lock (_handlers)
            {
                foreach (var handler in _handlers)
                {
                    handler.OnHandleMessage(message, context);

                    if (!context.Continue)
                        break;
                }
            }
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var context = new ValidationContext();
            lock (_handlers)
            {
                foreach (var validationRule in _handlers.OfType<IValidationRule>())
                {
                    validationRule.Validate(context);
                }
            }

            return context.Errors;
        }

        public void AddMessageHandler(ExpectationMessageHandler<TTopic> messageHandler, bool isSingleton = false)
        {
            if (messageHandler == null) throw new ArgumentNullException(nameof(messageHandler));

            lock (_handlers)
            {
                if (isSingleton)
                {
                    _handlers.RemoveAll(v => v.GetType() == messageHandler.GetType());
                }

                _handlers.Add(messageHandler);
            }
        }
    }
}