using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests
{
    public class RecordedMessageTemporalTests
    {
        public class Before
        {
            [Fact]
            public void Can_get_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var beforeTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.Before(beforeTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp < beforeTimeStamp));
            }
            
            [Fact]
            public void Can_get_generic_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var beforeTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.Before(beforeTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp < beforeTimeStamp));
            }
            
            [Fact]
            public void Can_get_messages_with_other_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var beforeMessage = messages.Skip(10).First();

                var filteredMessages = messages.Before(beforeMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(10);
                filteredMessages.Should().BeEquivalentTo(messages.Take(10));
            }
            
            [Fact]
            public void Can_get_messages_with_other_generic_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var beforeMessage = messages.Skip(10).First();

                var filteredMessages = messages.Before(beforeMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(10);
                filteredMessages.Should().BeEquivalentTo(messages.Take(10));
            }
        }
        
        public class BeforeOrOn
        {
            [Fact]
            public void Can_get_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var beforeOrOnTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.BeforeOrOn(beforeOrOnTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp <= beforeOrOnTimeStamp));
            }
            
            [Fact]
            public void Can_get_generic_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var beforeOrOnTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.BeforeOrOn(beforeOrOnTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp <= beforeOrOnTimeStamp));
            }
            
            [Fact]
            public void Can_get_messages_with_other_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var beforeOrOnMessage = messages.Skip(10).First();

                var filteredMessages = messages.BeforeOrOn(beforeOrOnMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(11);
                filteredMessages.Should().BeEquivalentTo(messages.Take(11));
            }
            
            [Fact]
            public void Can_get_messages_with_other_generic_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var beforeOrOnMessage = messages.Skip(10).First();

                var filteredMessages = messages.BeforeOrOn(beforeOrOnMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(11);
                filteredMessages.Should().BeEquivalentTo(messages.Take(11));
            }
        }
        
        public class After
        {
            [Fact]
            public void Can_get_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var afterTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.After(afterTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp > afterTimeStamp));
            }
            
            [Fact]
            public void Can_get_generic_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var afterTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.After(afterTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp > afterTimeStamp));
            }
            
            [Fact]
            public void Can_get_messages_with_other_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var afterMessage = messages.Skip(10).First();

                var filteredMessages = messages.After(afterMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(messages.Count() - 11);
                filteredMessages.Should().BeEquivalentTo(messages.Skip(11));
            }
            
            [Fact]
            public void Can_get_messages_with_other_generic_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var afterMessage = messages.Skip(10).First();

                var filteredMessages = messages.After(afterMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(messages.Count() - 11);
                filteredMessages.Should().BeEquivalentTo(messages.Skip(11));
            }
        }
        
        public class AfterOrOn
        {
            [Fact]
            public void Can_get_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var afterOrOnTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.AfterOrOn(afterOrOnTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp >= afterOrOnTimeStamp));
            }
            
            [Fact]
            public void Can_get_generic_messages_with_timestamp()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var afterOrOnTimeStamp = start.AddSeconds(30);

                var filteredMessages = messages.AfterOrOn(afterOrOnTimeStamp);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().BeEquivalentTo(messages.Where(x => x.TimeStamp >= afterOrOnTimeStamp));
            }
            
            [Fact]
            public void Can_get_messages_with_other_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var afterOrOnMessage = messages.Skip(10).First();

                var filteredMessages = messages.AfterOrOn(afterOrOnMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(messages.Count() - 10);
                filteredMessages.Should().BeEquivalentTo(messages.Skip(10));
            }
            
            [Fact]
            public void Can_get_messages_with_other_generic_message()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var afterOrOnMessage = messages.Skip(10).First();

                var filteredMessages = messages.AfterOrOn(afterOrOnMessage);

                filteredMessages.Should().NotBeNull();
                filteredMessages.Should().NotBeEmpty();
                filteredMessages.Should().HaveCount(messages.Count() - 10);
                filteredMessages.Should().BeEquivalentTo(messages.Skip(10));
            }
        }

        public class Duration
        {
            [Fact]
            public void Can_calculate_duration()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages(start, end, interval);

                var expectedDuration = end - start;

                messages.Duration().Should().Be(expectedDuration);
            }
            
            [Fact]
            public void Can_calculate_duration_for_generic_messages()
            {
                var start = new DateTime(2020, 7, 26, 13, 05, 00);
                var end = new DateTime(2020, 7, 26, 13, 05, 59);
                var interval = 1.Seconds();
                
                var messages = CreateMessages<object>(start, end, interval);

                var expectedDuration = end - start;

                messages.Duration().Should().Be(expectedDuration);
            }

            [Fact]
            public void Duration_of_empty_list_is_zero()
            {
                var target = new List<IRecordedMessage>();

                target.Duration().Should().Be(TimeSpan.Zero);
            }
            

            [Fact]
            public void Duration_of_null_list_is_zero()
            {
                IEnumerable<IRecordedMessage> target = null;

                target
                    .Invoking(x => x.Duration())
                    .Should()
                    .Throw<ArgumentNullException>();
            }

            [Fact]
            public void Duration_of_list_with_one_element_ist_zero()
            {
                var target = new List<IRecordedMessage>
                {
                    CreateMessage(DateTime.Now)
                };

                target.Duration().Should().Be(TimeSpan.Zero);
            }

            [Fact]
            public void Duration_of_empty_generic_list_is_zero()
            {
                var target = new List<IRecordedMessage<object>>();

                target.Duration().Should().Be(TimeSpan.Zero);
            }
            

            [Fact]
            public void Duration_of_null_generic_list_throws_exception()
            {
                IEnumerable<IRecordedMessage<object>> target = null;

                target
                    .Invoking(x => x.Duration())
                    .Should()
                    .Throw<ArgumentNullException>();
            }

            [Fact]
            public void Duration_of_generic_list_with_one_element_ist_zero()
            {
                var target = new List<IRecordedMessage<object>>
                {
                    CreateMessage<object>(DateTime.Now)
                };

                target.Duration().Should().Be(TimeSpan.Zero);
            }
        }

        private static IEnumerable<IRecordedMessage> CreateMessages()
        {
            var start = new DateTime(2020, 7, 26, 13, 05, 00);
            var end = new DateTime(2020, 7, 26, 13, 05, 59);
            var interval = TimeSpan.FromSeconds(1);
            
            return CreateMessages(start, end, interval);
        }

        private static IEnumerable<IRecordedMessage> CreateMessages(DateTime start, DateTime end, TimeSpan interval)
        {
            start = start.AsUtc();
            end = end.AsUtc();
            
            var messages = new List<IRecordedMessage>();

            for (var current = start; current <= end; current = current.Add(interval))
            {
                var message = CreateMessage(current);

                messages.Add(message);
            }

            return messages;
        }
        
        private static IEnumerable<IRecordedMessage<T>> CreateMessages<T>()
        {
            var start = new DateTime(2020, 7, 26, 13, 05, 00);
            var end = new DateTime(2020, 7, 26, 13, 05, 59);
            var interval = TimeSpan.FromSeconds(1);
            
            return CreateMessages<T>(start, end, interval);
        }
        
        private static IEnumerable<IRecordedMessage<T>> CreateMessages<T>(DateTime start, DateTime end, TimeSpan interval)
        {
            start = start.AsUtc();
            end = end.AsUtc();
            
            var messages = new List<IRecordedMessage<T>>();

            for (var current = start; current <= end; current = current.Add(interval))
            {
                var message = CreateMessage<T>(current);

                messages.Add(message);
            }

            return messages;
        }

        private static IRecordedMessage CreateMessage(DateTime timeStamp)
        {
            var mock = new Mock<IRecordedMessage>();

            mock.SetupGet(x => x.TimeStamp).Returns(timeStamp);
            mock.As<ITimestampMessage>().SetupGet(x => x.TimeStamp).Returns(timeStamp);

            var message = mock.Object;
            return message;
        }

        private static IRecordedMessage<T> CreateMessage<T>(DateTime timeStamp)
        {
            var mock = new Mock<IRecordedMessage<T>>();

            mock.SetupGet(x => x.TimeStamp).Returns(timeStamp);
            mock.As<ITimestampMessage>().SetupGet(x => x.TimeStamp).Returns(timeStamp);

            var message = mock.Object;
            return message;
        }
    }
}