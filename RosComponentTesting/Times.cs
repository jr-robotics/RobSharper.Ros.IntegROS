using System;

namespace RosComponentTesting
{
    public class Times
    {
        public static readonly Times Never = new Times(0, 0);
        public static readonly Times Once = new Times(1, 1);

        public static Times AtLeast(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value must not be negative");
            }
            
            return new Times((uint) value, uint.MaxValue);
        }

        public static Times AtMost(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value must not be negative");
            }
            
            return new Times(0, (uint) value);
        }
        
        
        public uint Min { get; }
        
        public uint Max { get; }

        public Times(uint min, uint max)
        {
            Min = min;
            Max = max;
        }

        public bool Evaluate(int value)
        {
            return Min <= value && Max >= value;
        }
        
        public bool Evaluate(uint value)
        {
            return Min <= value && Max >= value;
        }

        public bool Evaluate(long value)
        {
            return Min <= value && Max >= value;
        }

        public bool Evaluate(ulong value)
        {
            return Min <= value && Max >= value;
        }
    }
}