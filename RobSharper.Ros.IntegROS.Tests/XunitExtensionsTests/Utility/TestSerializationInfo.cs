using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace RobSharper.Ros.IntegROS.Tests.XunitExtensionsTests.Utility
{
    internal class TestSerializationInfo : IXunitSerializationInfo
    {
        private readonly IDictionary<string, object> _data = new Dictionary<string, object>();

        public TestSerializationInfo(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Type Type { get; }
            
        public void AddValue(string key, object value, Type type = null)
        {
            _data.Add(key, value);
        }

        public object GetValue(string key, Type type)
        {
            if (_data.TryGetValue(key, out var data))
            {
                return data;
            }

            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }

        public T GetValue<T>(string key)
        {
            return (T) GetValue(key, typeof(T));
        }

        public static TestSerializationInfo Serialize(IXunitSerializable serializableObject)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
                
            var info = new TestSerializationInfo(serializableObject.GetType());
            serializableObject.Serialize(info);

            return info;
        }

        public object Deserialize()
        {
            var deserializedObject = (IXunitSerializable) Activator.CreateInstance(Type);
            deserializedObject.Deserialize(this);

            return deserializedObject;
        }
    }
}