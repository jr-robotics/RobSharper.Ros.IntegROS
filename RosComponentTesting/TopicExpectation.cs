using System;
using System.Collections.Generic;
using System.Linq;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    public class TopicExpectation<TTopic> : ITopicExpectation
    {
        public bool Active { get; private set; }

        public virtual bool IsValid
        {
            get { return Validate().IsValid; }
        }

        public string TopicName { get; set; }
        
        public Type TopicType { get; set; }

        public event EventHandler<MessageReceivedArgs> OnMessageHandeled;

        private readonly List<ExpectationMessageHandler<TTopic>> _handlers = new List<ExpectationMessageHandler<TTopic>>();

        public virtual void Activate()
        {
            if (Active) return;
            
            lock (_handlers)
            {
                if (Active) return;
                
                foreach (var handler in _handlers)
                {
                    handler.OnActivateExpectation();
                }

                Active = true;
            }
        }

        public virtual void Deactivate()
        {
            if (!Active) return;

            lock (_handlers)
            {
                if (!Active) return;
                
                foreach (var handler in _handlers)
                {
                    handler.OnDeactivateExpectation();
                }

                Active = false;
            }
        }

        public void HandleMessage(object message)
        {
            if (!Active) return;
            
            HandleMessageInternal((TTopic)message);
            OnMessageHandeled?.Invoke(this, new MessageReceivedArgs(message));
        }

        protected virtual void HandleMessageInternal(TTopic message)
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

        public IEnumerable<ValidationError> GetValidationErrors()
        {
            var context = Validate();
            return context.Errors;
        }
        
        public bool AllMessageHandlersProcessed { get; private set; }
        
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

        private ValidationContext Validate()
        {
            var context = new ValidationContext();
            lock (_handlers)
            {
                foreach (var validationRule in _handlers.OfType<IValidationRule>())
                {
                    validationRule.Validate(context);
                }
            }

            return context;
        }
    }
}