using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace RobSharper.Ros.IntegROS
{
    public class RosNameRegexCache : IDisposable
    {
        private readonly ConcurrentDictionary<string, Regex> _cache = new ConcurrentDictionary<string, Regex>();
        
        public Regex GetOrCreate(string pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var regex = _cache.GetOrAdd(pattern, RosNameRegex.Create);
            return regex;
        }

        public void Dispose()
        {
            _cache.Clear();
        }
    }
}