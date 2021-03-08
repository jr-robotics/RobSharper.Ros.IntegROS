using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.XunitExtensions
{
    public static class ScenarioAttributeHelpers
    {
        /// <summary>
        /// Returns a merged list of <see cref="ScenarioAttribute"/> attributes defined at the test method or test class.
        /// If a scenario is defined at both levels, the method level attribute is used (and the class level attribute removed).
        ///
        /// If an exception is thrown during merging, the method falls back to a simple concatenated list of <see cref="ScenarioAttribute"/> 
        /// </summary>
        /// <param name="testMethod"></param>
        /// <param name="diagnosticsMessageSink"></param>
        /// <returns></returns>
        public static IEnumerable<IAttributeInfo> GetScenarioAttributes(this ITestMethod testMethod,
            IMessageSink diagnosticsMessageSink)
        {
            var methodScenarios = testMethod.Method.GetCustomAttributes(typeof(ScenarioAttribute));
            var classScenarios = testMethod.TestClass.Class.GetCustomAttributes(typeof(ScenarioAttribute));
            
            try
            {
                return MergeScenarioAttributes(methodScenarios, classScenarios);
            }
            catch (Exception e)
            {
                diagnosticsMessageSink.OnMessage(new DiagnosticMessage(
                    $"Exception thrown while merging scenario attributes for '{testMethod.TestClass.Class.Name}.{testMethod.Method.Name}'; falling back to unfiltered list of scenario attributes.{Environment.NewLine}{e}"));
            }

            // Fallback is a concatenated list.
            return methodScenarios
                .Concat(classScenarios)
                .ToList();
        }
        
        /// <summary>
        /// Returns a merged list of <see cref="ScenarioAttribute"/> attributes defined at the test method or test class.
        /// If a scenario is defined at both levels, the method level attribute is used (and the class level attribute removed). 
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        public static IEnumerable<IAttributeInfo> GetScenarioAttributes(this ITestMethod testMethod)
        {
            var methodScenarios = testMethod.Method.GetCustomAttributes(typeof(ScenarioAttribute));
            var classScenarios = testMethod.TestClass.Class.GetCustomAttributes(typeof(ScenarioAttribute));

            return MergeScenarioAttributes(methodScenarios, classScenarios);
        }
        
        /// <summary>
        /// Merges scenario attributes defined at the method and class levels.
        ///
        /// Eliminates duplicate entries (Method level attributes are in favor). 
        /// </summary>
        /// <param name="methodAttributeInfos"></param>
        /// <param name="classAttributeInfos"></param>
        /// <returns></returns>
        private static IEnumerable<IAttributeInfo> MergeScenarioAttributes(IEnumerable<IAttributeInfo> methodAttributeInfos, IEnumerable<IAttributeInfo> classAttributeInfos)
        {
            var mergedAttributeInfos = methodAttributeInfos?.ToList() ?? new List<IAttributeInfo>();

            if (classAttributeInfos == null || !classAttributeInfos.Any())
                return mergedAttributeInfos;

            var methodScenarioAttributes = mergedAttributeInfos
                .OfType<IReflectionAttributeInfo>()
                .Select(x => x.Attribute)
                .Cast<ScenarioAttribute>()
                .ToList();
            
            foreach (var classAttributeInfo in classAttributeInfos)
            {
                if (classAttributeInfo is IReflectionAttributeInfo classReflectionAttributeInfo)
                {
                    var classScenarioAttribute = (ScenarioAttribute) classReflectionAttributeInfo.Attribute;
                    var isDuplicate = methodScenarioAttributes
                        .Any(methodScenarioAttribute => methodScenarioAttribute.ScenarioEquals(classScenarioAttribute));

                    if (isDuplicate)
                        continue;
                }
                
                mergedAttributeInfos.Add(classAttributeInfo);
            }
            
            return mergedAttributeInfos;
        }
    }
}