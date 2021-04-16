using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public static class RecordedMessageNamespaceExtensions
    {
        public static IEnumerable<string> ExpandNamespaces(string globalTopicName)
        {
            if (globalTopicName == null) throw new ArgumentNullException(nameof(globalTopicName));
            if (!globalTopicName.StartsWith("/")) throw new ArgumentException("Global topic name must start with '/'.", nameof(globalTopicName));

            var namespaceChunks = globalTopicName.Split('/');
            var namespaceBuilder = new StringBuilder();
            var namespaces = new List<string>();
            
            var appendSlash = true;

            for (int i = 0; i < namespaceChunks.Length - 1; i++)
            {
                if (appendSlash)
                    namespaceBuilder.Append("/");

                var namespaceChunk = namespaceChunks[i];
                
                namespaceBuilder.Append(namespaceChunk);
                appendSlash = namespaceChunk != String.Empty;
                
                
                namespaces.Add(namespaceBuilder.ToString());
            }

            return namespaces;
        }

        /// <summary>
        /// Creates a regex, which can check if a global ros topic name matches the given namespace pattern.
        /// </summary>
        /// <param name="namespacePattern"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static Regex CreateNamespaceRegex(string namespacePattern)
        {
            if (namespacePattern == null) throw new ArgumentNullException(nameof(namespacePattern));
            
            if (!namespacePattern.EndsWith(RosNameRegex.AnyPlaceholder))
            {
                if (!namespacePattern.EndsWith("/"))
                {
                    namespacePattern += "/";
                }

                namespacePattern += RosNameRegex.AnyPlaceholder;
            }

            var regex = RosNameRegex.Create(namespacePattern);
            return regex;
        }
        
        public static bool IsInNamespace(this IRecordedMessage message, string namespacePattern)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var regex = CreateNamespaceRegex(namespacePattern);
            return regex.IsMatch(message.Topic);
        }
        
        public static INamespaceScopedRecordedMessage InNamespace(this IRecordedMessage message,
            string namespacePattern)
        {
            if (!IsInNamespace(message, namespacePattern))
                throw new InvalidOperationException($"Message is not in namespace {namespacePattern}.");

            return NamespaceScopedRecordedMessage.Create(message, namespacePattern);
        }

        public static IEnumerable<IRecordedMessage> InNamespace(this IEnumerable<IRecordedMessage> messages,
            string namespacePattern)
        {
            var regex = CreateNamespaceRegex(namespacePattern);
            return messages
                .Where(m => regex.IsMatch(m.Topic))
                .Select(m => NamespaceScopedRecordedMessage.Create(m, namespacePattern));
        }

        public static IEnumerable<IGrouping<string, IRecordedMessage>> GroupByNamespace(this IEnumerable<IRecordedMessage> messages)
        {
            return messages
                .SelectMany(m =>
                {
                    return ExpandNamespaces(m.Topic)
                        .Select(ns => new
                        {
                            Namespace = ns,
                            Message = m
                        });
                })
                .GroupBy(x => x.Namespace, x => x.Message);
        }

        public static IEnumerable<IGrouping<string, IRecordedMessage>> GroupByNamespace(this IEnumerable<IRecordedMessage> messages, string namespacePattern)
        {
            var regex = RosNameRegex.Create(namespacePattern);

            var filteredNamespaces = messages
                .GroupByNamespace()
                .Where(g => regex.IsMatch(g.Key));

            return filteredNamespaces;
        }
    }

    public interface INamespaceScopedRecordedMessage : IRecordedMessage
    {
        string NamespaceScope { get; }
    }

    public class NamespaceScopedRecordedMessage : INamespaceScopedRecordedMessage
    {
        public string NamespaceScope { get; }
        private readonly IRecordedMessage _recordedMessage;

        private NamespaceScopedRecordedMessage(IRecordedMessage recordedMessage, string namespaceScope)
        {
            NamespaceScope = namespaceScope ?? throw new ArgumentNullException(nameof(namespaceScope));
            _recordedMessage = recordedMessage ?? throw new ArgumentNullException(nameof(recordedMessage));
        }

        public DateTime TimeStamp => _recordedMessage.TimeStamp;
        public string Topic => _recordedMessage.Topic;
        public RosType Type => _recordedMessage.Type;
        public object GetMessage(Type type)
        {
            return _recordedMessage.GetMessage(type);
        }

        public TType GetMessage<TType>()
        {
            return _recordedMessage.GetMessage<TType>();
        }

        public static NamespaceScopedRecordedMessage Create(IRecordedMessage recordedMessage, string namespaceScope)
        {
            if (recordedMessage == null)
                return null;

            // TODO: check for inner scope
            return new NamespaceScopedRecordedMessage(recordedMessage, namespaceScope);
        }
    }
}