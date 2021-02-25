using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility
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
        
        public static TestClass TestClass(Type type)
        {
            var collection = TestCollection(type.GetTypeInfo().Assembly);

            return new TestClass(collection, Reflector.Wrap(type));
        }
        
        public static ITestMethod TestMethod(Type type, string methodName, string displayName = null, string skip = null, int timeout = 0)
        {
            var @class = TestClass(type);
            var methodInfo = type.GetMethod(methodName);
            
            if (methodInfo == null)
                throw new Exception($"Unknown method: {type.FullName}.{methodName}");

            var expectAttribute = new ExpectThatAttribute()
            {
                DisplayName = displayName,
                Skip = skip,
                Timeout = timeout
            };
            
            var testMethodInfo = new ExpectatThatTestMethodInfo(methodInfo, expectAttribute);
            
            return new TestMethod(@class, testMethodInfo);
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

    public class ExpectatThatTestMethodInfo : IReflectionMethodInfo, IMethodInfo
    {
        private readonly IReflectionMethodInfo _reflectionMethodInfo;
        private readonly IReflectionAttributeInfo _expectAttribute;

        public ExpectatThatTestMethodInfo(MethodInfo methodInfo, ExpectThatAttribute expectAttribute)
        {
            _reflectionMethodInfo = Reflector.Wrap(methodInfo);
            
            var expectAttributeMock = new Mock<IReflectionAttributeInfo>();

            expectAttributeMock.SetupGet(x => x.Attribute).Returns(expectAttribute);
            expectAttributeMock.Setup(x => x.GetNamedArgument<string>("DisplayName"))
                .Returns(expectAttribute.DisplayName);
            expectAttributeMock.Setup(x => x.GetNamedArgument<string>("Skip")).Returns(expectAttribute.Skip);
            expectAttributeMock.Setup(x => x.GetNamedArgument<int>("Timeout")).Returns(expectAttribute.Timeout);
            
            _expectAttribute = expectAttributeMock.Object;
        }

        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            var attributeType = System.Type.GetType(assemblyQualifiedAttributeTypeName);
            if (attributeType != null && attributeType.IsAssignableFrom(typeof(ExpectThatAttribute)))
            {
                return new[] {_expectAttribute};
            }

            return _reflectionMethodInfo.GetCustomAttributes(assemblyQualifiedAttributeTypeName);
        }

        public IEnumerable<ITypeInfo> GetGenericArguments()
        {
            return _reflectionMethodInfo.GetGenericArguments();
        }

        public IEnumerable<IParameterInfo> GetParameters()
        {
            return _reflectionMethodInfo.GetParameters();
        }

        public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments)
        {
            return _reflectionMethodInfo.MakeGenericMethod(typeArguments);
        }

        public bool IsAbstract => _reflectionMethodInfo.IsAbstract;
        public bool IsGenericMethodDefinition => _reflectionMethodInfo.IsGenericMethodDefinition;
        public bool IsPublic => _reflectionMethodInfo.IsPublic;
        public bool IsStatic => _reflectionMethodInfo.IsStatic;
        public string Name => _reflectionMethodInfo.Name;
        public ITypeInfo ReturnType => _reflectionMethodInfo.ReturnType;
        public ITypeInfo Type => _reflectionMethodInfo.Type;
        public MethodInfo MethodInfo => _reflectionMethodInfo.MethodInfo;
    }
}