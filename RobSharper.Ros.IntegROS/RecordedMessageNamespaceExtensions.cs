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
        private static Regex CreateNamespaceRegex(this IRecordedMessage message, string namespacePattern)
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

            if (message is INamespaceScopedRecordedMessage namespaceMessage)
            {
                if (!RosNameRegex.IsGlobalPattern(namespacePattern))
                {
                    namespacePattern = namespaceMessage.NamespaceScope + "/" + namespacePattern;
                }
                else
                {
                    throw new InvalidTopicPatternException(
                        "Cannot apply two global namespace patterns to one message.");
                }
            }

            var regex = RosNameRegex.Create(namespacePattern);
            return regex;
        }
        
        public static bool IsInNamespace(this IRecordedMessage message, string namespacePattern)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            var regex = message.CreateNamespaceRegex(namespacePattern);
            return regex.IsMatch(message.Topic);
        }

        public static IEnumerable<IRecordedMessage> InNamespace(this IEnumerable<IRecordedMessage> messages,
            string namespacePattern)
        {
            return messages
                .Where(m => m.IsInNamespace(namespacePattern))
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
        
        IRecordedMessage InnerMessage { get; }
    }

    public static class INamespaceScopedRecordedMessageExtensions
    {
        public static IRecordedMessage Unwrap(this INamespaceScopedRecordedMessage message)
        {
            if (message == null)
                return null;

            var unwrappedMessage = message.InnerMessage;

            while (unwrappedMessage is INamespaceScopedRecordedMessage unwrappedNamespaceMessage)
            {
                unwrappedMessage = unwrappedNamespaceMessage.InnerMessage;
            }

            return unwrappedMessage;
        }
    }

    public class NamespaceScopedRecordedMessage : INamespaceScopedRecordedMessage
    {
        public string NamespaceScope { get; }
        public IRecordedMessage InnerMessage { get; }

        private NamespaceScopedRecordedMessage(IRecordedMessage recordedMessage, string namespaceScope)
        {
            NamespaceScope = namespaceScope ?? throw new ArgumentNullException(nameof(namespaceScope));
            InnerMessage = recordedMessage ?? throw new ArgumentNullException(nameof(recordedMessage));
        }

        public DateTime TimeStamp => InnerMessage.TimeStamp;
        public string Topic => InnerMessage.Topic;
        public RosType Type => InnerMessage.Type;
        public object GetMessage(Type type)
        {
            return InnerMessage.GetMessage(type);
        }

        public TType GetMessage<TType>()
        {
            return InnerMessage.GetMessage<TType>();
        }

        public static NamespaceScopedRecordedMessage Create(IRecordedMessage recordedMessage, string namespaceScope)
        {
            if (recordedMessage == null)
                return null;

            if (recordedMessage is INamespaceScopedRecordedMessage namespaceScopedMessage)
            {
                namespaceScope = namespaceScopedMessage.NamespaceScope + "/" + namespaceScope;
                recordedMessage = namespaceScopedMessage.Unwrap();
            }
            
            return new NamespaceScopedRecordedMessage(recordedMessage, namespaceScope);
        }
    }
}