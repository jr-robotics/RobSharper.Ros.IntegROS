using System;
using System.Collections.Generic;
using System.Linq;
using RobSharper.Ros.BagReader;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace RobSharper.Ros.IntegROS.Ros.Rosbag
{
    public class RobSharperRosbagReaderAdapter : IRosbagReader
    {
        private readonly RosMessageSerializer _serializer;

        public RosMessageSerializer Serializer => _serializer;

        public RobSharperRosbagReaderAdapter(RosMessageSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public IEnumerable<IRecordedMessage> Read(string filename)
        {
            //TODO: Maybe switch to other bag type (FileBag) if file size gets to big
            // to be handeled in memory?
            //var mem = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
            
            var bag = FileBag.Create(filename);
            return bag.Messages
                .Select(m => new RobSharperRecordedBagMessage(m, _serializer));
        }
    }
}