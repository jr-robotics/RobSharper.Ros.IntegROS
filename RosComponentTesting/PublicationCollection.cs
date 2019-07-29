using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RosComponentTesting
{
    public class PublicationCollection : Collection<IPublication>
    {
        private class TopicInfoCounter
        {
            public TopicDescriptor Topic { get; set; }
            
            public int PublicationsCount { get; set; }
        }
        
        private readonly Dictionary<TopicDescriptor, TopicInfoCounter> _topics = new Dictionary<TopicDescriptor, TopicInfoCounter>();

        public IEnumerable<TopicDescriptor> Topics
        {
            get
            {
                return _topics
                    .Values
                    .Where(x => x.PublicationsCount > 0)
                    .Select(x => x.Topic)
                    .ToList();
            }
        }
        
        protected override void ClearItems()
        {
            base.ClearItems();
            _topics.Clear();
        }

        protected override void InsertItem(int index, IPublication item)
        {
            if (base.IndexOf(item) > -1)
            {
                throw new InvalidOperationException("Item is already in the list");
            }
            
            base.InsertItem(index, item);

            var topicInfo = GetTopicInfo(item);
            topicInfo.PublicationsCount++;
        }

        protected override void RemoveItem(int index)
        {
            var item = Items[index];
            
            base.RemoveItem(index);
            
            var topicInfo = GetTopicInfo(item);
            topicInfo.PublicationsCount--;
        }

        protected override void SetItem(int index, IPublication item)
        {
            var itemIndex = base.IndexOf(item);
            if (itemIndex > -1 && itemIndex != index)
            {
                throw new InvalidOperationException("Item is already in the list");
            }
            
            var oldItem = base.Items[index];
            var oldTopicInfo = GetTopicInfo(oldItem);
            oldTopicInfo.PublicationsCount--;
            
            base.SetItem(index, item);

            var topicInfo = GetTopicInfo(item);
            topicInfo.PublicationsCount++;
        }

        private TopicInfoCounter GetTopicInfo(IPublication item)
        {
            TopicInfoCounter topicInfo;

            if (!_topics.TryGetValue(item.Topic, out topicInfo))
            {
                topicInfo = new TopicInfoCounter()
                {
                    Topic = item.Topic
                };
                
                _topics[item.Topic] = topicInfo;
            }

            return topicInfo;
        }
    }
}