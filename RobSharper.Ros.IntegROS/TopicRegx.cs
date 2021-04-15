using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RobSharper.Ros.IntegROS
{
    public class TopicRegx
    {
        private const string AnyPlaceholder = "**";
        private static readonly Regex InvalidAnyPlaceholder = new Regex("([A-z0-9_]\\*\\*|\\*\\*[A-z0-9_])");
        
        public static Regex Create(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            pattern = pattern
                .Trim();
            
            if (string.Empty.Equals(pattern))
                throw new InvalidTopicPatternException("Topic name pattern must not be empty", nameof(pattern));

            if (!pattern.StartsWith("/") && !pattern.StartsWith(AnyPlaceholder))
                throw new InvalidTopicPatternException("Topic name pattern must be in global format (start with '/')");
            
            if (pattern.EndsWith("/"))
                throw new InvalidTopicPatternException("Topic name pattern must not end with a namespace separator ('/')");

            if (InvalidAnyPlaceholder.IsMatch(pattern))
                throw new InvalidTopicPatternException("Topic name pattern is invalid (invalid use of ** placeholder)",
                    nameof(pattern));
            
            pattern = pattern
                .Replace(AnyPlaceholder, "[[ANY]]")
                .Replace("*", "[[PARTIAL]]")
                .Replace("[[ANY]]", ".*")
                .Replace("[[PARTIAL]]", "[0-9A-Za-z_]*");

            var regex = new StringBuilder();
            regex.Append('^');
            
            regex.Append(pattern);
            regex.Append('$');

            return new Regex(regex.ToString());
        }
    }
}