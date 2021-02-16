using System;
using System.Reflection;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegROS.Tests.XunitExtensionsTests.Utility
{
    public class XunitMocks
    {

        public static TestAssembly TestAssembly(Assembly assembly = null, string configFileName = null)
        {
#if NETFRAMEWORK
            if (configFileName == null)
                configFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif

            return new TestAssembly(Reflector.Wrap(assembly ?? typeof(XunitMocks).GetTypeInfo().Assembly), configFileName);
        }
        
        public static TestCollection TestCollection(Assembly assembly = null, ITypeInfo definition = null, string displayName = null)
        {
            if (assembly == null)
                assembly = typeof(XunitMocks).GetTypeInfo().Assembly;
            if (displayName == null)
                displayName = "Mock test collection for " + assembly.CodeBase;

            return new TestCollection(TestAssembly(assembly), definition, displayName);
        }
        
        public static TestClass TestClass(Type type, ITestCollection collection = null)
        {
            if (collection == null)
                collection = TestCollection(type.GetTypeInfo().Assembly);

            return new TestClass(collection, Reflector.Wrap(type));
        }
        
        public static ITestMethod TestMethod(Type type, string methodName, ITestCollection collection = null)
        {
            var @class = TestClass(type, collection);
            var methodInfo = type.GetMethod(methodName);
            if (methodInfo == null)
                throw new Exception($"Unknown method: {type.FullName}.{methodName}");

            return new TestMethod(@class, Reflector.Wrap(methodInfo));
        }

        public static IReflectionAttributeInfo ExpectThatAttribute(string displayName = null, string skip = null, int timeout = 0)
        {
            var attribute = new ExpectThatAttribute();
            
            if (displayName != null)
                attribute.DisplayName = displayName;

            if (skip != null)
                attribute.Skip = skip;

            if (timeout != 0)
                attribute.Timeout = timeout;
            
            var mock = new Mock<IReflectionAttributeInfo>(MockBehavior.Strict);

            mock.SetupGet(x => x.Attribute).Returns(attribute);
            mock.Setup(x => x.GetNamedArgument<string>("DisplayName")).Returns(attribute.DisplayName);
            mock.Setup(x => x.GetNamedArgument<string>("Skip")).Returns(attribute.Skip);
            mock.Setup(x => x.GetNamedArgument<int>("Timeout")).Returns(attribute.Timeout);
            
            return mock.Object;
        }
    }
}