namespace IntegROS.Rosbag
{
    public static class RosbagReader
    {
        public static IRosbagReader Instance { get; set; } = new RobSharperRosbagReaderAdapter();
    }
}