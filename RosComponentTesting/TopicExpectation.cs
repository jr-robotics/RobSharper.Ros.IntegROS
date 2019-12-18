using System;
using System.Collections.Generic;
using System.Linq;
using RosComponentTesting.MessageHandling;

namespace RosComponentTesting
{
    
    public class TopicExpectation<TTopic> : ITopicExpectation
    {
        private readonly List<SortableMessageHandlerListItem<TTopic>> _handlers = new List<SortableMessageHandlerListItem<TTopic>>();

        public bool IsActive { get; private set; }

        public string TopicName { get; set; }
        
        public Type TopicType => typeof(TTopic);

        public virtual bool IsValid => Validate().IsValid;

        public virtual void Activate()
        {
            if (IsActive) return;
            
            lock (_handlers)
            {
                if (IsActive) return;
                
                foreach (var handler in _handlers)
                {
                    handler.Item.Activate();
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
                    handler.Item.Deactivate();
                }

                IsActive = false;
            }
        }
        
        public void HandleMessage(object message)
        {
            if (!IsActive) return;
            
            
            lock (_handlers)
            {
                var handlers = _handlers.Select(h => h.Item);
                HandleMessageInternal((TTopic) message, handlers);
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
        
        public void AddMessageHandler(MessageHandlerBase<TTopic> messageHandler, int priority, bool isSingleton = false)
        {
            if (messageHandler == null) throw new ArgumentNullException(nameof(messageHandler));

            lock (_handlers)
            {
                if (isSingleton)
                {
                    _handlers.RemoveAll(v => v.GetType() == messageHandler.GetType());
                }

                var item = new SortableMessageHandlerListItem<TTopic>(messageHandler, priority);
                
                _handlers.Add(item);
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