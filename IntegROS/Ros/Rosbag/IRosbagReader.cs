using System.Collections.Generic;

namespace IntegROS.Ros.Rosbag
{
    public interface IRosbagReader
    {
        IEnumerable<IRecordedMessage> Read(string filename);
    }
}