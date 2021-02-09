using System;
using System.Collections.Generic;
using IntegROS.Rosbag;

namespace IntegROS
{
    public class RosbagScenario : IScenario
    {
        private readonly string _bagfile;
        private IEnumerable<IRecordedMessage> _messages;

        public IEnumerable<IRecordedMessage> Messages
        {
            get
            {
                if (_messages == null)
                {
                    Load();
                }
                
                return _messages;
            }
        }

        public string RosbagFile
        {
            get => _bagfile;
        }

        public RosbagScenario(string bagfile)
        {
            _bagfile = bagfile ?? throw new ArgumentNullException(nameof(bagfile));
        }

        private void Load()
        {
            _messages = RosbagReader.Instance.Read(_bagfile);
        }
    }
}