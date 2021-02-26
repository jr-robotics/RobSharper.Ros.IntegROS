using System;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.IntegROS
{
    public static class RecordedMessageTemporalExtensions
    {
        public static TimeSpan Duration(this IEnumerable<ITimestampMessage> messages)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));
            
            if (!messages.Any())
                return TimeSpan.Zero;
            
            var first = messages.First();
            var last = messages.Last();

            return last.TimeStamp - first.TimeStamp;
        }

        #region Before
        
        public static IEnumerable<IRecordedMessage> Before(this IEnumerable<IRecordedMessage> messages, DateTime timeStamp)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                if (message.TimeStamp >= timeStamp)
                    yield break;

                yield return message;
            }
        }

        public static IEnumerable<IRecordedMessage> Before(this IEnumerable<IRecordedMessage> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return Before(messages, other.TimeStamp);
        }

        public static IEnumerable<IRecordedMessage<T>> Before<T>(this IEnumerable<IRecordedMessage<T>> messages, DateTime timeStamp)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                if (message.TimeStamp >= timeStamp)
                    yield break;

                yield return message;
            }
        }

        public static IEnumerable<IRecordedMessage<T>> Before<T>(this IEnumerable<IRecordedMessage<T>> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return Before(messages, other.TimeStamp);
        }
        
        #endregion
        
        #region BeforeOrOn

        public static IEnumerable<IRecordedMessage> BeforeOrOn(this IEnumerable<IRecordedMessage> messages, DateTime timeStamp)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                if (message.TimeStamp > timeStamp)
                    yield break;

                yield return message;
            }
        }

        public static IEnumerable<IRecordedMessage> BeforeOrOn(this IEnumerable<IRecordedMessage> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return BeforeOrOn(messages, other.TimeStamp);
        }

        public static IEnumerable<IRecordedMessage<T>> BeforeOrOn<T>(this IEnumerable<IRecordedMessage<T>> messages, DateTime timeStamp)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            foreach (var message in messages)
            {
                if (message.TimeStamp > timeStamp)
                    yield break;

                yield return message;
            }
        }

        public static IEnumerable<IRecordedMessage<T>> BeforeOrOn<T>(this IEnumerable<IRecordedMessage<T>> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return BeforeOrOn(messages, other.TimeStamp);
        }
        
        #endregion

        #region After
        
        public static IEnumerable<IRecordedMessage> After(this IEnumerable<IRecordedMessage> messages, DateTime timeStamp)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            return messages.SkipWhile(x => x.TimeStamp <= timeStamp);
        }

        public static IEnumerable<IRecordedMessage> After(this IEnumerable<IRecordedMessage> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return After(messages, other.TimeStamp);
        }

        public static IEnumerable<IRecordedMessage<T>> After<T>(this IEnumerable<IRecordedMessage<T>> messages, DateTime timeStamp)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            return messages.SkipWhile(x => x.TimeStamp <= timeStamp);
        }

        public static IEnumerable<IRecordedMessage<T>> After<T>(this IEnumerable<IRecordedMessage<T>> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return After(messages, other.TimeStamp);
        }
        
        #endregion
        
        #region AfterOrOn
        
        public static IEnumerable<IRecordedMessage> AfterOrOn(this IEnumerable<IRecordedMessage> messages, DateTime timeStamp)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            return messages.SkipWhile(x => x.TimeStamp < timeStamp);
        }

        public static IEnumerable<IRecordedMessage> AfterOrOn(this IEnumerable<IRecordedMessage> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return AfterOrOn(messages, other.TimeStamp);
        }

        public static IEnumerable<IRecordedMessage<T>> AfterOrOn<T>(this IEnumerable<IRecordedMessage<T>> messages, DateTime timeStamp)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            return messages.SkipWhile(x => x.TimeStamp < timeStamp);
        }

        public static IEnumerable<IRecordedMessage<T>> AfterOrOn<T>(this IEnumerable<IRecordedMessage<T>> messages, ITimestampMessage other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            return AfterOrOn(messages, other.TimeStamp);
        }
        
        #endregion
    }
}