using System;

namespace RosComponentTesting
{
    public class Times
    {
        public static readonly Times Never = new Times(0, 0);
        public static readonly Times Once = new Times(1, 1);

        public static Times AtLeast(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            
            return AtLeast((uint) value);
        }
        
        public static Times AtLeast(uint value)
        {
            return new Times((uint) value, uint.MaxValue);
        }

        public static Times AtMost(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            return AtMost((uint) value);
        }

        public static Times AtMost(uint value)
        {
            return new Times(0, value);
        }

        public static Times Exactly(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            
            return Exactly((uint) value);
        }

        public static Times Exactly(uint value)
        {
            return new Times(value, value);
        }

        public static Times Between(int min, int max)
        {
            if (min < 0) throw new ArgumentOutOfRangeException(nameof(min));
            if (max < 0) throw new ArgumentOutOfRangeException(nameof(max));

            return Between((uint) min, (uint) max);
        }

        public static Times Between(uint min, uint max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException((nameof(min)));
            }
            
            return new Times(min, max);
        }
        
        public uint Min { get; }
        
        public uint Max { get; }

        private Times(uint min, uint max)
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
        protected bool Equals(Times other)
        {
            return Min == other.Min && Max == other.Max;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Times) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Min * 397) ^ (int) Max;
            }
        }
    }
}