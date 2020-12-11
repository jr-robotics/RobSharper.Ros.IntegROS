using System.Collections.Generic;

namespace IntegROS.Rosbag
{
    public interface IRosbagReader
    {
        IEnumerable<RecordedMessage> Read(string filename);
    }
}