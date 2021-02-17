using System;
using System.ComponentModel;
using IntegROS.XunitExtensions.ScenarioDiscovery;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class NullScenarioIdentifier : ScenarioIdentifierBase
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public NullScenarioIdentifier()
        {
        }
        
        public NullScenarioIdentifier(Type scenarioDiscovererType, string displayName = null) : base(scenarioDiscovererType, displayName)
        {
        }
            
        protected override string GetUniqueId()
        {
            return NullScenarioAttribute.NullId.ToString("D");
        }
    }
}