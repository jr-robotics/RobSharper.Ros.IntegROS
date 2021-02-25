using System;
using System.ComponentModel;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery
{
    public class RosbagScenarioIdentifier : ScenarioIdentifierBase
    {
        public string Bagfile { get; private set; }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public RosbagScenarioIdentifier() { }
        
        public RosbagScenarioIdentifier(string bagfile, Type scenarioDiscovererType, string displayName) : base(scenarioDiscovererType, displayName)
        {
            Bagfile = bagfile;
        }

        public override void Deserialize(IXunitSerializationInfo info)
        {
            Bagfile = info.GetValue<string>(nameof(Bagfile));
            
            base.Deserialize(info);
        }

        public override void Serialize(IXunitSerializationInfo info)
        {
            base.Serialize(info);
            
            info.AddValue(nameof(Bagfile), Bagfile);
        }

        protected override string GetUniqueId()
        {
            return $"ROSBAG:{Bagfile}";
        }
    }
}