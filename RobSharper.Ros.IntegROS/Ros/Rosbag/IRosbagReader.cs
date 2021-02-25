using System.Collections.Generic;

namespace RobSharper.Ros.IntegROS.Ros.Rosbag
{
    public interface IRosbagReader
    {
        IEnumerable<IRecordedMessage> Read(string filename);
    }
}