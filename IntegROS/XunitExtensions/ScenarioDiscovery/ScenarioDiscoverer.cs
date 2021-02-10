﻿using System;
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
                return null;

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