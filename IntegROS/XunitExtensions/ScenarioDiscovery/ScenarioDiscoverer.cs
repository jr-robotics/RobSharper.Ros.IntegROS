using System;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.XunitExtensions.ScenarioDiscovery
{
    public static class ScenarioDiscovererFactory
    {
        public static IScenarioDiscoverer GetDiscoverer(IMessageSink diagnosticsMessageSink, IAttributeInfo scenarioAttribute)
        {
            var discovererAttribute = scenarioAttribute.GetCustomAttributes(typeof(ScenarioDiscovererAttribute)).First();
            var args = discovererAttribute.GetConstructorArguments().Cast<string>().ToList();
            var discovererType = LoadType(args[1], args[0]);

            if (discovererType == null)
                throw new InvalidOperationException(
                    $"Could not load scenario discoverer type \"{args[0]}, {args[1]}\"");
            
            return GetDiscoverer(diagnosticsMessageSink, discovererType);
        }

        public static IScenarioDiscoverer GetDiscoverer(IMessageSink diagnosticsMessageSink,
            IScenarioIdentifier scenarioIdentifier)
        {
            return GetDiscoverer(diagnosticsMessageSink, scenarioIdentifier.ScenarioDiscovererType);
        }

        private static IScenarioDiscoverer GetDiscoverer(IMessageSink diagnosticsMessageSink, Type discovererType)
        {
            if (discovererType == null)
                throw new ArgumentNullException(nameof(discovererType));

            return ExtensibilityPointFactory.Get<IScenarioDiscoverer>(diagnosticsMessageSink, discovererType);
        }

        private static Type LoadType(string assemblyName, string typeName)
        {
#if NETFRAMEWORK
            // Support both long name ("assembly, version=x.x.x.x, etc.") and short name ("assembly")
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName || a.GetName().Name == assemblyName);
            if (assembly == null)
            {
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch { }
            }
#else
            Assembly assembly = null;
            try
            {
                // Make sure we only use the short form
                var an = new AssemblyName(assemblyName);
                assembly = Assembly.Load(new AssemblyName { Name = an.Name, Version = an.Version });

            }
            catch { }
#endif

            if (assembly == null)
                return null;

            return assembly.GetType(typeName);
        }
    }
}