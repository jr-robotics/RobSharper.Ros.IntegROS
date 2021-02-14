using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace IntegROS.XunitExtensions
{
    public static class ScenarioAttributeHelpers
    {
        public static IEnumerable<IAttributeInfo> GetScenarioAttributes(this ITestMethod testMethod)
        {
            var methodScenarios = testMethod.Method.GetCustomAttributes(typeof(ScenarioAttribute));
            var classScenarios = testMethod.TestClass.Class.GetCustomAttributes(typeof(ScenarioAttribute));

            return MergeScenarioAttributes(methodScenarios, classScenarios);
        }
        
        public static IEnumerable<IAttributeInfo> MergeScenarioAttributes(IEnumerable<IAttributeInfo> methodAttributeInfos, IEnumerable<IAttributeInfo> classAttributeInfos)
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