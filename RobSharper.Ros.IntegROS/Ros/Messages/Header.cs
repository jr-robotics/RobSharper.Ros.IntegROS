using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.Messages
{
    [RosMessage("std_msgs/Header")]
    public class Header
    {
        [RosMessageField("uint32", "seq", 1)]
        public uint SequenceId { get; set; }
        
        [RosMessageField("time", "stamp", 2)]
        public DateTime Stamp { get; set; }
        
        [RosMessageField("string", "frame_id", 3)]
        public string FrameId { get; set; }
    }
}