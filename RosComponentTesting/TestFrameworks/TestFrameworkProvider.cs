using System;
using System.Collections.Generic;

namespace RosComponentTesting.TestFrameworks
{
    public class TestFrameworkProvider
    {
        /// <summary>
        /// Built in list of supported test frameworks.
        /// </summary>
        private static readonly List<ITestFramework> _frameworks = new List<ITestFramework>()
        {
            new Xunit2TestFramework()
        };
        
        private static ITestFramework _instance;

        /// <summary>
        /// Returns the actual active test framework or a Fallback test framework if
        /// the test framework could not be detected.
        /// </summary>
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
                            _instance = GetTestFramework() ?? new FallbackTestFramework();
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

            return null;
        }

        /// <summary>
        /// Adds a new test framework to the list of possible frameworks.
        /// </summary>
        /// <param name="framework"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddTestFramework(ITestFramework framework)
        {
            if (framework == null) throw new ArgumentNullException(nameof(framework));

            _frameworks.Add(framework);
            _instance = null;
        }
    }
}