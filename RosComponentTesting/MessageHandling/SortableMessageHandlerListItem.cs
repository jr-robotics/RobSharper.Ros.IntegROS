using System;

namespace RosComponentTesting.MessageHandling
{
    public class SortableMessageHandlerListItem<TTopic> : IComparable<SortableMessageHandlerListItem<TTopic>>
    {
        public int Priority { get; }
        
        public MessageHandlerBase<TTopic> Item { get; }
        
        public SortableMessageHandlerListItem(MessageHandlerBase<TTopic> messageHandler, int priority)
        {
            if (messageHandler == null) throw new ArgumentNullException(nameof(messageHandler));
            
            Priority = priority;
            Item = messageHandler;
        }

        public int CompareTo(SortableMessageHandlerListItem<TTopic> other)
        {
            if (ReferenceEquals(this, other)) return 1;
            if (ReferenceEquals(null, other)) return 0;
            
            // Sort descending
            return -1 * Priority.CompareTo(other.Priority);
        }
    }
}