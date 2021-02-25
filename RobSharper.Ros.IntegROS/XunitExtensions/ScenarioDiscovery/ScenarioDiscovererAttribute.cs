using System;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.XunitExtensions.ScenarioDiscovery
{
    /// <summary>
    /// An attribute used to decorate classes which derive from <see cref="DataAttribute"/>,
    /// to indicate how scenarios should be discovered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ScenarioDiscovererAttribute : Attribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="ScenarioDiscovererAttribute"/>.
        /// </summary>
        /// <param name="typeName">The fully qualified type name of the discoverer</param>
        /// <param name="assemblyName">The name of the assembly that the discoverer type is located</param>
        public ScenarioDiscovererAttribute(string typeName, string assemblyName) { }
    }
}