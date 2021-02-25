using System.Collections.Generic;
using Xunit.Abstractions;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class TestFrameworkOptions : ITestFrameworkDiscoveryOptions
    {
        private readonly Dictionary<string, object> _options = new Dictionary<string, object>();
        
        public TValue GetValue<TValue>(string name)
        {
            if (_options.TryGetValue(name, out var result))
                return (TValue)result;

            return default(TValue);
        }

        public void SetValue<TValue>(string name, TValue value)
        {
            if (value == null)
                _options.Remove(name);
            else
                _options[name] = value;
        }
    }
}