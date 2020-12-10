using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegROS
{
    public class RosbagScenario : IScenario
    {
        private IQueryable<RecordedMessage> _messages;

        public IQueryable<RecordedMessage> Messages
        {
            get
            {
                if (_messages == null)
                {
                    throw new InvalidOperationException("Messages have not been loaded yet. Call Load(rosbagFile) before accessing messages.");
                }
                return _messages;
            }
        }

        public string RosbagFile { get; private set; }

        public void Load(string rosbagFile)
        {
            if (rosbagFile == null) throw new ArgumentNullException(nameof(rosbagFile));

            if (_messages != null)
            {
                if (!string.Equals(RosbagFile, rosbagFile))
                    throw new InvalidOperationException("Another rosbag was loaded before.");
                
                return;
            }
            
            _messages = RosbagReader.Instance.Read(rosbagFile).AsQueryable();
            RosbagFile = rosbagFile;
        }
    }

    public interface IRosbagReader
    {
        IEnumerable<RecordedMessage> Read(string filename);
    }

    public static class RosbagReader
    {
        public static IRosbagReader Instance { get; set; }
    }
}