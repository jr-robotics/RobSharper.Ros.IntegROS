using System;
using System.Text.RegularExpressions;

namespace RobSharper.Ros.IntegROS
{
    public class NamespacePattern
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

        public NamespacePattern(string namespacePattern)
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

        public bool Equals(NamespacePattern other)
        {
            return Pattern == other.Pattern;
        }

        public override bool Equals(object obj)
        {
            return obj is NamespacePattern other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Pattern != null ? Pattern.GetHashCode() : 0);
        }

        public NamespacePattern Concat(NamespacePattern other)
        {
            return Concat(this, other);
        }

        public bool IsMatch(string value)
        {
            if (!IsGlobalPattern)
                throw new InvalidOperationException("Cannot match relative namespace pattern.");
            
            return Regex.IsMatch(value);
        }

        public static NamespacePattern Concat(NamespacePattern first, NamespacePattern second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            
            if (second.IsGlobalPattern)
            {
                throw new InvalidRosNamePatternException(
                    "Cannot append a global namespace pattern to another pattern.");
            }
            
            var ns = first.Pattern + "/" + second.Pattern;
            return new NamespacePattern(ns);
        }
    }
}