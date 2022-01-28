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
            
            if (string.Empty.Equals(pattern))
                throw new InvalidRosNamePatternException("ROS name pattern must not be empty", nameof(pattern));

            if (pattern.EndsWith("/"))
                throw new InvalidRosNamePatternException(
                    "ROS name pattern must not end with a namespace separator ('/')", nameof(pattern));

            if (!IsGlobalPattern(pattern))
                throw new InvalidRosNamePatternException("ROS name pattern must be in global format (start with '/')", nameof(pattern));

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

        /// <summary>
        /// Determines if a ROS name pattern is in global format (i.e. starts with / or **) 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsGlobalPattern(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return pattern.StartsWith("/") || pattern.StartsWith(AnyPlaceholder);
        }

        /// <summary>
        /// Determines if a ROS name pattern contains Placeholders (* or **)
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ContainsPlaceholders(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return pattern.Contains(AnyPlaceholder) || pattern.Contains(PartialPlaceholder);
        }

        /// <summary>
        /// Determines if a ROS name pattern is fully qualified.
        /// </summary>
        /// <remarks>A name is fully qualified if it is in ROS global format and does not contain any placeholders.</remarks>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsFullQualifiedPattern(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            return IsGlobalPattern(pattern) && !ContainsPlaceholders(pattern);
        }
    }
}