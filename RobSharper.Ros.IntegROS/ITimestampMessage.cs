using System;

namespace RobSharper.Ros.IntegROS
{
    public interface ITimestampMessage
    {
        DateTime TimeStamp { get; }
    }
}