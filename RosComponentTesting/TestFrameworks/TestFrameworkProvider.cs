using System;
using System.Collections.Generic;

namespace RosComponentTesting.TestFrameworks
{
    public class TestFrameworkProvider
    {
        private static List<ITestFramework> _frameworks = new List<ITestFramework>()
        {
            new Xunit2TestFramework(),
            new FallbackTestFramework()
        };
        
        private static ITestFramework _instance;

        public static ITestFramework Framework
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(TestFrameworkProvider))
                    {
                        if (_instance == null)
                        {
                            _instance = GetTestFramework();
                        }
                    }
                }
                
                return _instance;
            }
        }

        private static ITestFramework GetTestFramework()
        {
            foreach (var testFramework in _frameworks)
            {
                if (testFramework.IsLoaded)
                {
                    return testFramework;
                }
            }

            throw new NotSupportedException("No valid test framework found.");
        }
    }
}