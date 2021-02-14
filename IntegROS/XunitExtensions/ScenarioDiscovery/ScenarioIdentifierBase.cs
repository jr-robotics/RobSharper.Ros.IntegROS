using System;
using System.ComponentModel;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public abstract class ScenarioIdentifierBase : IScenarioIdentifier
    {
        private string _uniqueId;

        public string UniqueId
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
        
        public Type ScenarioDiscovererType { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        protected ScenarioIdentifierBase()
        {
        }
        
        protected ScenarioIdentifierBase(Type scenarioDiscovererType)
        {
            ScenarioDiscovererType = scenarioDiscovererType;
        }

        public override string ToString()
        {
            return UniqueId;
        }

        public virtual void Deserialize(IXunitSerializationInfo info)
        {
            _uniqueId = info.GetValue<string>(nameof(UniqueId));

            var discovererTypeName = info.GetValue<string>(nameof(ScenarioDiscovererType));
            if (discovererTypeName != null)
                ScenarioDiscovererType = Type.GetType(discovererTypeName);
        }

        public virtual void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(UniqueId), UniqueId);
            info.AddValue(nameof(ScenarioDiscovererType), ScenarioDiscovererType?.AssemblyQualifiedName);
        }

        protected abstract string GetUniqueId();
    }
}