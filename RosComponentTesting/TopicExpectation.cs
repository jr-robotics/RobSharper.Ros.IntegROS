using System;
using System.Collections.Generic;
using System.Linq;
using RosComponentTesting.MessageHandling;

namespace RosComponentTesting
{
    
    public class TopicExpectation<TTopic> : ITopicExpectation
    {
        private readonly List<MessageHandlerBase<TTopic>> _handlers = new List<MessageHandlerBase<TTopic>>();

        public bool IsActive { get; private set; }

        public string TopicName { get; set; }
        
        public Type TopicType { get; set; }

        public virtual bool IsValid
        {
            get { return Validate().IsValid; }
        }

        public virtual void Activate()
        {
            if (IsActive) return;
            
            lock (_handlers)
            {
                if (IsActive) return;
                
                foreach (var handler in _handlers)
                {
                    handler.Activate();
                }

                IsActive = true;
            }
        }

        public virtual void Deactivate()
        {
            if (!IsActive) return;

            lock (_handlers)
            {
                if (!IsActive) return;
                
                foreach (var handler in _handlers)
                {
                    handler.Deactivate();
                }

                IsActive = false;
            }
        }
        
        public void HandleMessage(object message)
        {
            if (!IsActive) return;
            
            
            lock (_handlers)
            {
                HandleMessageInternal((TTopic) message, _handlers);
            }
        }

        protected virtual void HandleMessageInternal(TTopic message,
            IEnumerable<MessageHandlerBase<TTopic>> handlers)
        {
            var context = new MessageHandlingContext();
                
            foreach (var handler in handlers)
            {
                handler.HandleMessage(message, context);

                if (!context.Continue)
                    break;
            }
        }

        public IEnumerable<ValidationError> GetValidationErrors()
        {
            var context = Validate();
            return context.Errors;
        }
        
        public void AddMessageHandler(MessageHandlerBase<TTopic> messageHandler, bool isSingleton = false)
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
                foreach (var validationRule in _handlers.OfType<IValidationMessageHandler>())
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