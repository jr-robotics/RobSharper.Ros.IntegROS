using System;
using System.Reflection;

namespace RosComponentTesting.TestFrameworks
{
    internal class Xunit2TestFramework : ITestFramework
    {
        private static Assembly _testFrameworkAssembly;
        private static Type _exceptionType;

        public bool IsLoaded
        {
            get
            {
                try
                {
                    return TestFrameworkAssembly != null;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static Assembly TestFrameworkAssembly
        {
            get
            {
                if (_testFrameworkAssembly == null)
                {
                    _testFrameworkAssembly = Assembly.Load(new AssemblyName("xunit.assert"));
                }
                
                return _testFrameworkAssembly;
            }
        }

        private static Type ExceptionType
        {
            get
            {
                if (_exceptionType == null)
                {
                    _exceptionType = TestFrameworkAssembly.GetType("Xunit.Sdk.XunitException");
                }
                
                return _exceptionType;
            }
        }

        public bool IsTestException(Exception e)
        {
            return ExceptionType.IsInstanceOfType(e);
        }

        public void Throw(string errorMessage)
        {
            throw (Exception) Activator.CreateInstance(ExceptionType, errorMessage);
        }
    }
}