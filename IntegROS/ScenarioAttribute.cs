using System;

namespace IntegROS
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ScenarioAttribute : Attribute
    {
        public string DisplayName { get; set; }
        
        public string Skip { get; set; }
        
    }
}