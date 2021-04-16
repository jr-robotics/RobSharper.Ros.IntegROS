using System;
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
    }
}