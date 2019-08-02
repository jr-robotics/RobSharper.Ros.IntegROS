using System;
using System.Collections.Generic;
using System.Linq;
using RosComponentTesting.ExpectationProcessing;

namespace RosComponentTesting
{
    
    public class TopicExpectation<TTopic> : ITopicExpectation
    {
        private readonly List<ExpectationMessageHandler<TTopic>> _handlers = new List<ExpectationMessageHandler<TTopic>>();

        public bool Active { get; private set; }

        public string TopicName { get; set; }
        
        public Type TopicType { get; set; }

        public virtual bool IsValid
        {
            get { return Validate().IsValid; }
        }

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
            
            
            lock (_handlers)
            {
                HandleMessageInternal((TTopic) message, _handlers);
            }
        }

        protected virtual void HandleMessageInternal(TTopic message,
            IEnumerable<ExpectationMessageHandler<TTopic>> handlers)
        {
            var context = new ExpectationRuleContext();
                
            foreach (var handler in handlers)
            {
                handler.OnHandleMessage(message, context);

                if (!context.Continue)
                    break;
            }
        }

        public IEnumerable<ValidationError> GetValidationErrors()
        {
            var context = Validate();
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
                _handlers.Sort();
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

        public virtual void Cancel()
        {
            Deactivate();
        }
    }
}