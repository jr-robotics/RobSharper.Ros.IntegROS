using System;

namespace RosComponentTesting
{
    public class Times
    {
        public static readonly Times Never = new Times(0, 0);
        public static readonly Times Once = new Times(1, 1);

        public static Times AtLeast(uint value)
        {
            return new Times((uint) value, uint.MaxValue);
        }

        public static Times AtMost(uint value)
        {
            return new Times(0, value);
        }

        public static Times Exactly(uint value)
        {
            return new Times(value, value);
        }
        
        
        public uint Min { get; }
        
        public uint Max { get; }

        public Times(uint min, uint max)
        {
            Min = min;
            Max = max;
        }

        public bool IsValid(long value)
        {
            return Min <= value && Max >= value;
        }

        public bool IsValid(ulong value)
        {
            return Min <= value && Max >= value;
        }
    }
}