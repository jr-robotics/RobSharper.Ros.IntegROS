using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        private static NamespaceScope CreateNamespaceScope(this IRecordedMessage message, string namespacePattern)
        {
            var scope = new NamespaceScope(namespacePattern);

            if (message is INamespaceScopedRecordedMessage namespaceMessage)
            {
                scope = namespaceMessage.NamespaceScope.Concat(scope);
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

    public interface INamespaceScopedRecordedMessage : IRecordedMessage
    {
        NamespaceScope NamespaceScope { get; }
        
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

    public class NamespaceScope
    {
        private Regex _regex;
        
        public string Pattern { get; }

        public bool IsGlobalPattern => RosNameRegex.IsGlobalPattern(Pattern);

        private Regex Regex
        {
            get
            {
                if (_regex == null)
                {
                    InitializeRegex();
                }

                return _regex;
            }
        }

        public NamespaceScope(string namespacePattern)
        {
            Pattern = namespacePattern ?? throw new ArgumentNullException(nameof(namespacePattern));
        }

        private void InitializeRegex()
        {
            var namespacePattern = Pattern;
            
            if (!namespacePattern.EndsWith(RosNameRegex.AnyPlaceholder))
            {
                if (!namespacePattern.EndsWith("/"))
                {
                    namespacePattern += "/";
                }

                namespacePattern += RosNameRegex.AnyPlaceholder;
            }

            _regex = RosNameRegex.Create(namespacePattern);
        }

        public override string ToString()
        {
            return Pattern;
        }

        public bool Equals(NamespaceScope other)
        {
            return Pattern == other.Pattern;
        }

        public override bool Equals(object obj)
        {
            return obj is NamespaceScope other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Pattern != null ? Pattern.GetHashCode() : 0);
        }

        public NamespaceScope Concat(NamespaceScope other)
        {
            return Concat(this, other);
        }

        public bool IsMatch(string value)
        {
            if (!IsGlobalPattern)
                throw new InvalidOperationException("Cannot match relative namespace pattern");
            
            return Regex.IsMatch(value);
        }

        public static NamespaceScope Concat(NamespaceScope first, NamespaceScope second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            
            if (second.IsGlobalPattern)
            {
                throw new InvalidRosNamePatternException(
                    "Cannot append a global namespace pattern to another pattern.");
            }
            
            var ns = first.Pattern + "/" + second.Pattern;
            return new NamespaceScope(ns);
        }
    }
}