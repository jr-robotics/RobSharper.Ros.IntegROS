using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ScenarioAttribute : Attribute
    {
        public string DisplayName { get; set; }
        
        public string Skip { get; set; }

        public virtual bool ScenarioEquals(ScenarioAttribute other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;

            return GetScenarioHashCode() == other.GetScenarioHashCode();
        }

        public abstract int GetScenarioHashCode();

        protected bool Equals(ScenarioAttribute other)
        {
            return base.Equals(other) && 
                   DisplayName == other.DisplayName && 
                   Skip == other.Skip &&
                   GetScenarioHashCode() == other.GetScenarioHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScenarioAttribute) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DisplayName, Skip);
        }
        

        internal static string GetAttributeTypeName(IAttributeInfo scenarioAttribute)
        {
            if (scenarioAttribute is IReflectionAttributeInfo reflectionAttribute)
            {
                var attribute = reflectionAttribute.Attribute;
                if (attribute != null)
                {
                    return attribute.ToString();
                }
            }

            return "(unknown attribute type)";
        }

        internal static string GetAttributeDefinition(IAttributeInfo scenarioAttribute)
        {
            if (scenarioAttribute is ReflectionAttributeInfo xunitReflectionAttribute)
            {
                var attributeData = xunitReflectionAttribute.AttributeData;
                if (attributeData != null)
                    return attributeData.ToString();
            }

            if (scenarioAttribute is IReflectionAttributeInfo reflectionAttribute)
            {
                var attribute = reflectionAttribute.Attribute;
                if (attribute != null)
                {
                    return $"[{attribute.GetType()}(???)]";
                }
            }
            
            return "[???ScenarioAttribute(???)]";
        }
    }
}