using System;
using System.ComponentModel;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public abstract class ScenarioIdentifierBase : IScenarioIdentifier
    {
        private string _uniqueId;

        public string DisplayName { get; protected set; }

        public string UniqueScenarioId
        {
            get
            {
                if (_uniqueId == null)
                {
                    _uniqueId = GetUniqueId();
                }

                return _uniqueId;
            }
        }
        
        public Type ScenarioDiscovererType { get; protected set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        protected ScenarioIdentifierBase()
        {
        }
        
        protected ScenarioIdentifierBase(Type scenarioDiscovererType, string displayName = null)
        {
            ScenarioDiscovererType = scenarioDiscovererType;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(DisplayName))
                return UniqueScenarioId;
            
            return DisplayName;
        }

        public virtual void Deserialize(IXunitSerializationInfo info)
        {
            _uniqueId = info.GetValue<string>(nameof(UniqueScenarioId));
            DisplayName = info.GetValue<string>(nameof(DisplayName));

            var discovererTypeName = info.GetValue<string>(nameof(ScenarioDiscovererType));
            if (discovererTypeName != null)
                ScenarioDiscovererType = Type.GetType(discovererTypeName);
        }

        public virtual void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(UniqueScenarioId), UniqueScenarioId);
            info.AddValue(nameof(DisplayName), DisplayName);
            info.AddValue(nameof(ScenarioDiscovererType), ScenarioDiscovererType?.AssemblyQualifiedName);
        }

        protected abstract string GetUniqueId();
    }
}