using System;
using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public abstract class MessageHandlerBase<TTopic> : IComparable<MessageHandlerBase<TTopic>>
    {
        public bool IsActive { get; private set; }
        public int Priority { get; }

        public CallerReference CallerInfo { get;}

        public MessageHandlerBase(int priority) : this(null, priority)
        {
        }

        protected MessageHandlerBase(CallerReference callerInfo, int priority)
        {
            Priority = priority;
            CallerInfo = callerInfo;
        }

        public virtual void Activate()
        {
            IsActive = true;
        }

        public virtual void Deactivate()
        {
            IsActive = false;
        }

        public void HandleMessage(TTopic message, MessageHandlingContext context)
        {
            if (!IsActive)
                return;

            HandleMessageInternal(message, context);
        }
        
        protected abstract void HandleMessageInternal(TTopic message, MessageHandlingContext context);

        public int CompareTo(MessageHandlerBase<TTopic> other)
        {
            if (ReferenceEquals(this, other)) return 1;
            if (ReferenceEquals(null, other)) return 0;
            
            // Sort descending
            return -1 * Priority.CompareTo(other.Priority);
        }
    }
}