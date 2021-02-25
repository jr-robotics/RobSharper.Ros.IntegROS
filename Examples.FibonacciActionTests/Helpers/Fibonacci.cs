using System.Collections.Generic;

namespace Examples.FibonacciActionTests.Helpers
{
    public static class Fibonacci
    {
        public static IEnumerable<int> CalculateSequence(int count)
        {
            var result = new int[count];

            if (count > 0)
                result[0] = 0;

            if (count > 1)
                result[1] = 1;

            for (var i = 2; i < count; i++)
            {
                result[i] = result[i - 1] + result[i - 2];
            }

            return result;
        }
    }
}