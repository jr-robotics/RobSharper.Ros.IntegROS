using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RobSharper.Ros.IntegROS
{
    public class RosNameRegex
    {
        public const string PartialPlaceholder = "*";
        public const string AnyPlaceholder = "**";
        
        public static Regex Create(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            pattern = pattern
                .Trim();
            
            if (string.Empty.Equals(pattern))
                throw new InvalidTopicPatternException("ROS name pattern must not be empty", nameof(pattern));

            if (!pattern.StartsWith("/") && !pattern.StartsWith(AnyPlaceholder))
                throw new InvalidTopicPatternException("ROS name pattern must be in global format (start with '/')");
            
            if (pattern.EndsWith("/"))
                throw new InvalidTopicPatternException("ROS name pattern must not end with a namespace separator ('/')");
            
            pattern = pattern
                .Replace(AnyPlaceholder, "[[ANY]]")
                .Replace(PartialPlaceholder, "[[PARTIAL]]")
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