using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private static NamespacePattern CreateNamespaceScope(this IRecordedMessage message, string namespacePattern)
        {
            var scope = new NamespacePattern(namespacePattern);

            if (message is INamespaceScopedRecordedMessage namespaceMessage)
            {
                scope = namespaceMessage.NamespacePattern.Concat(scope);
            }

            return scope;
        }
        
        public static bool IsInNamespace(this IRecordedMessage message, string namespacePattern)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var namespaceScope = message.CreateNamespaceScope(namespacePattern);
            return namespaceScope.IsMatch(message.Topic);
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
}