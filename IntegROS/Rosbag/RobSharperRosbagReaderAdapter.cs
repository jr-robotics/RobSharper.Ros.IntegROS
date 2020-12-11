using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RobSharper.Ros.BagReader;

namespace IntegROS.Rosbag
{
    public class RobSharperRosbagReaderAdapter : IRosbagReader
    {
        public IEnumerable<RecordedMessage> Read(string filename)
        {
            //TODO: Maybe switch to other bag type (FileBag) if file size gets to big
            // to be handeled in memory?
            //var mem = Process.GetCurrentProcess().PrivateMemorySize64;
            
            var bag = FileBag.Create(filename);
            return bag.Messages
                .Select(m => new RecordedMessage());
        }
    }
}