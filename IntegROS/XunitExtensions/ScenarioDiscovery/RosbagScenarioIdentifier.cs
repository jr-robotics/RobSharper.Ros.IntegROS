using System;
using System.ComponentModel;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public class RosbagScenarioIdentifier : IScenarioIdentifier
    {
        public string Bagfile { get; }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public RosbagScenarioIdentifier() { }
        
        public RosbagScenarioIdentifier(string bagfile)
        {
            Bagfile = bagfile;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            info.GetValue<string>("Bagfile");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Bagfile", Bagfile);
        }

        public override string ToString()
        {
            return Bagfile;
        }
    }
}