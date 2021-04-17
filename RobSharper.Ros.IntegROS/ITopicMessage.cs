using RobSharper.Ros.MessageEssentials;

namespace RobSharper.Ros.IntegROS
{
    public interface ITopicMessage
    {
        RosType Type { get; }
        
        string Topic { get; }
    }
}