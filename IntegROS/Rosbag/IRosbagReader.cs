using System.Collections.Generic;

namespace IntegROS.Rosbag
{
    public interface IRosbagReader
    {
        IEnumerable<IRecordedMessage> Read(string filename);
    }
}