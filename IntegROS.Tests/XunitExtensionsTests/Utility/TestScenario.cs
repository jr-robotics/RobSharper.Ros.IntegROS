using System.Collections.Generic;
using IntegROS.Scenarios;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class TestScenario : IScenario
    {
        private List<IRecordedMessage> _messages = new List<IRecordedMessage>();
        
        public IEnumerable<IRecordedMessage> Messages => _messages;
    }
}