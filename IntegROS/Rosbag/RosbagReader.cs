using RobSharper.Ros.MessageEssentials;
using RobSharper.Ros.MessageEssentials.Serialization;

namespace IntegROS.Rosbag
{
    public static class RosbagReader
    {
        // TODO: Avoid default hardcoded initialization
        public static IRosbagReader Instance { get; set; } = new RobSharperRosbagReaderAdapter(new RosMessageSerializer(new MessageTypeRegistry()));
    }
}