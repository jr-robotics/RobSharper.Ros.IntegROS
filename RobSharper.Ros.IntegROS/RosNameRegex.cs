using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RobSharper.Ros.IntegROS
{
    public static class RosNameRegex
    {
        public const string PartialPlaceholder = "*";
        public const string AnyPlaceholder = "**";
        
        public static Regex Create(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            pattern = pattern.Trim();
            
            AssertValidPattern(pattern);

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

        internal static void AssertValidPattern(string pattern)
        {
            if (pattern == null) 
                throw new ArgumentNullException(nameof(pattern));
            
            if (string.Empty.Equals(pattern))
                throw new InvalidRosNamePatternException("ROS name pattern must not be empty", nameof(pattern));

            if (!IsGlobalPattern(pattern))
                throw new InvalidRosNamePatternException("ROS name pattern must be in global format (start with '/')", nameof(pattern));

            if (pattern.EndsWith("/"))
                throw new InvalidRosNamePatternException(
                    "ROS name pattern must not end with a namespace separator ('/')", nameof(pattern));
        }

        public static bool IsGlobalPattern(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return pattern.StartsWith("/") || pattern.StartsWith(AnyPlaceholder);
        }

        public static bool ContainsPlaceholders(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return pattern.Contains(AnyPlaceholder) || pattern.Contains(PartialPlaceholder);
        }
    }
}