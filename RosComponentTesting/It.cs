using System;

namespace RosComponentTesting
{
    public static class It
    {
        public static Match<T> IsAny<T>()
        {
            return new Match<T>(m => m != null && m.GetType() == typeof(T));
        }

        public static Match<T> Matches<T>(Func<T, bool> func)
        {
            return new Match<T>(func);
        }
    }
}