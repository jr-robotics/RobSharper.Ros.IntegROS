using System;

namespace RosComponentTestingTests.Util
{
    public class It
    {
        public static Func<T,bool> IsAny<T>()
        {
            return m => m != null && m.GetType() == typeof(T);
        }

        public static Func<T,bool> Matches<T>(Func<T, bool> func)
        {
            return func;
        }
    }
}