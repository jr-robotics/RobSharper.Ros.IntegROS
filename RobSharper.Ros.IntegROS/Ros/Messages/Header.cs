using System;
using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS.Ros.Messages
{
    [RosMessage("std_msgs/Header")]
    public class Header
    {
        [RosMessageField(1, "uint32", "seq")]
        public uint SequenceId { get; set; }
        
        [RosMessageField(2, "time", "stamp")]
        public DateTime Stamp { get; set; }
        
        [RosMessageField(3, "string", "frame_id")]
        public string FrameId { get; set; }
    }
}