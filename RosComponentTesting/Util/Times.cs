namespace RosComponentTesting.Util
{
    public class Times
    {
        public int Min { get; }
        
        public int Max { get; }

        public Times(int min, int max)
        {
            Min = min;
            Max = max;
        }
        
        public static Times Once()
        {
            return new Times(1, 1);
        }

        public static Times Never()
        {
            return new Times(0, 0);
        }
    }
}