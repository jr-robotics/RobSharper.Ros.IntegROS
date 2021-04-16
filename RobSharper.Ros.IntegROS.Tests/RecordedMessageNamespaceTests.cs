using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace RobSharper.Ros.IntegROS.Tests
{
    public class RecordedMessageNamespaceTests
    {
        [Theory]
        [InlineData("/TopicName", new [] {"/"})]
        [InlineData("/A/TopicName", new [] {"/", "/A"})]
        [InlineData("/A/B/TopicName", new [] {"/", "/A", "/A/B"})]
        [InlineData("/A/B/C/TopicName", new [] {"/", "/A", "/A/B", "/A/B/C"})]
        public void Namespaces_can_be_expanded(string topicName, IEnumerable<string> expectedNamespaces)
        {
            var actualNamespaces = RecordedMessageNamespaceExtensions.ExpandNamespaces(topicName);

            actualNamespaces.Should().BeEquivalentTo(expectedNamespaces);
        }

        [Fact]
        public void Namespace_cannot_be_expanded_for_null_topic_name()
        {
            Action call = () => RecordedMessageNamespaceExtensions.ExpandNamespaces(null);

            call.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Namespace_cannot_be_expanded_for_empty_topic_name()
        {
            Action call = () => RecordedMessageNamespaceExtensions.ExpandNamespaces(string.Empty);

            call.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Namespace_cannot_be_expanded_for_relative_topic_name()
        {
            Action call = () => RecordedMessageNamespaceExtensions.ExpandNamespaces("RelativeNamespace/Topic");

            call.Should().Throw<ArgumentException>();
        }

        
        [Fact]
        public void Can_group_messages_by_namespace()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/A/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/A/B/Topic"),
                CreateMessage("/B/Topic"),
                CreateMessage("/A/B/C/Topic"),
                CreateMessage("/Topic")
            };

            var namespaceGroupings = messages.GroupByNamespace();

            namespaceGroupings.Should().NotBeNull();

            var namespaces = namespaceGroupings
                .Select(n => n.Key)
                .ToList();

            namespaces.Should().NotBeNullOrEmpty();
            namespaces.Should().BeEquivalentTo(new[]
            {
                "/",
                "/A",
                "/A/B",
                "/A/B/C",
                "/B"
            });
        }
        
        [Fact]
        public void Namespace_groupings_contain_all_messages_from_namespace_and_sub_namespaces()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/A/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/A/B/Topic"),
                CreateMessage("/B/Topic"),
                CreateMessage("/A/B/C/Topic"),
                CreateMessage("/Topic")
            };

            var namespaceGroupings = messages.GroupByNamespace();

            namespaceGroupings.Should().NotBeNull();

            foreach (var namespaceGrouping in namespaceGroupings)
            {
                var namespaceName = namespaceGrouping.Key;
                var expectedMessages = messages
                    .Where(x => x.Topic.StartsWith(namespaceName))
                    .ToList();

                namespaceGrouping.Should().BeEquivalentTo(expectedMessages);
            }
        }
        
        
        [Theory]
        [InlineData("/A", new [] { "/A"})]
        [InlineData("/A/**", new [] { "/A/B", "/A/B/C"})]
        [InlineData("/A**", new [] { "/A", "/A/B", "/A/B/C"})]
        [InlineData("/NS*", new [] { "/NS1", "/NS2", "/NS3"})]
        [InlineData("/NS*/**", new [] { "/NS3/X", "/NS3/Y", "/NS3/X/X"})]
        [InlineData("/NS*/*", new [] { "/NS3/X", "/NS3/Y"})]
        [InlineData("/NS*/*/*", new [] { "/NS3/X/X"})]
        public void Can_group_messages_by_namespace_with_name(string filter, IEnumerable<string> expectedNamespaces)
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/A/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/A/B/Topic"),
                CreateMessage("/B/Topic"),
                CreateMessage("/A/B/C/Topic"),
                CreateMessage("/Topic"),
                CreateMessage("/NS1/Topic"),
                CreateMessage("/NS1/Topic"),
                CreateMessage("/NS1/Topic"),
                CreateMessage("/NS2/Topic"),
                CreateMessage("/NS3/Topic"),
                CreateMessage("/NS3/X/Topic"),
                CreateMessage("/NS3/Y/Topic"),
                CreateMessage("/NS3/X/X/Topic")
            };

            var namespaces = messages
                .GroupByNamespace(filter)
                .Select(n => n.Key)
                .ToList();

            namespaces.Should().NotBeNullOrEmpty();
            namespaces.Should().BeEquivalentTo(expectedNamespaces);
        }
        
        
        [Fact]
        public void Can_select_messages_in_namespace()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/turtle1/pose"),
                CreateMessage("/turtle2/pose"),
                CreateMessage("/turtle10/pose"),
                CreateMessage("/turtle11/pose"),
            };
            messages
                .InNamespace("/turtle1")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/turtle1/pose"
                });
        }
        
        [Fact]
        public void Can_select_messages_in_namespace_with_partial_placeholder()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/turtle1/pose"),
                CreateMessage("/turtle2/pose"),
                CreateMessage("/turtle10/pose"),
                CreateMessage("/turtle11/pose"),
            };

            messages
                .InNamespace("/turtle1*")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/turtle1/pose",
                    "/turtle10/pose",
                    "/turtle11/pose"
                });
        }
        
        [Fact]
        public void Can_select_messages_in_namespace_and_sub_namespace()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/a/pose"),
                CreateMessage("/b/pose"),
                CreateMessage("/c/pose"),
                CreateMessage("/a/a/pose"),
                CreateMessage("/a/b/pose"),
                CreateMessage("/a/c/pose"),
            };

            messages
                .InNamespace("/a")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/a/pose",
                    "/a/a/pose",
                    "/a/b/pose",
                    "/a/c/pose"
                });
        }
        
        [Fact]
        public void Can_select_messages_in_namespace_and_sub_namespace_with_partial_placeholder()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/turtle1/pose"),
                CreateMessage("/turtle2/pose"),
                CreateMessage("/turtle10/pose"),
                CreateMessage("/turtle11/pose"),
                CreateMessage("/turtle1/a/pose"),
                CreateMessage("/turtle2/a/pose"),
                CreateMessage("/turtle10/a/pose"),
                CreateMessage("/turtle11/a/pose"),
            };

            messages
                .InNamespace("/turtle1*")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/turtle1/pose",
                    "/turtle10/pose",
                    "/turtle11/pose",
                    "/turtle1/a/pose",
                    "/turtle10/a/pose",
                    "/turtle11/a/pose"
                });
        }

        [Fact]
        public void Can_select_messages_in_sub_namespace_with_partial_placeholder()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/turtle1/pose"),
                CreateMessage("/turtle2/pose"),
                CreateMessage("/turtle10/pose"),
                CreateMessage("/turtle11/pose"),
                CreateMessage("/turtle1/a/pose"),
                CreateMessage("/turtle2/a/pose"),
                CreateMessage("/turtle10/a/pose"),
                CreateMessage("/turtle11/a/pose"),
            };

            messages
                .InNamespace("/turtle1/*")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/turtle1/a/pose",
                });
        }

        [Fact]
        public void Can_select_messages_in_namespace_and_sub_namespace_with_any_placeholder()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/turtle1/pose"),
                CreateMessage("/turtle2/pose"),
                CreateMessage("/turtle10/pose"),
                CreateMessage("/turtle11/pose"),
                CreateMessage("/turtle1/a/pose"),
                CreateMessage("/turtle2/a/pose"),
                CreateMessage("/turtle10/a/pose"),
                CreateMessage("/turtle11/a/pose"),
            };

            messages
                .InNamespace("/turtle1**")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/turtle1/pose",
                    "/turtle10/pose",
                    "/turtle11/pose",
                    "/turtle1/a/pose",
                    "/turtle10/a/pose",
                    "/turtle11/a/pose"
                });
        }
        
        [Fact]
        public void Can_select_messages_in_sub_namespace_with_any_placeholder()
        {
            var messages = new List<IRecordedMessage>()
            {
                CreateMessage("/turtle1/pose"),
                CreateMessage("/turtle2/pose"),
                CreateMessage("/turtle10/pose"),
                CreateMessage("/turtle11/pose"),
                CreateMessage("/turtle1/a/pose"),
                CreateMessage("/turtle2/a/pose"),
                CreateMessage("/turtle10/a/pose"),
                CreateMessage("/turtle11/a/pose"),
            };

            messages
                .InNamespace("/turtle1/**")
                .Select(m => m.Topic)
                .Should()
                .BeEquivalentTo(new[]
                {
                    "/turtle1/pose",
                    "/turtle1/a/pose",
                });
        }
        

        private IRecordedMessage CreateMessage(string globalTopicName)
        {
            var mock = new Mock<IRecordedMessage>();
            mock.SetupGet(m => m.Topic).Returns(globalTopicName);

            var message = mock.Object;
            return message;
        }
    }
}