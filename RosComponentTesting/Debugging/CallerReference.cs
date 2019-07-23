namespace RosComponentTesting.Debugging
{
    public class CallerReference
    {
        public string CallerFilePath { get; }
        public int LineNumber { get; }

        private CallerReference(string callerFilePath, int lineNumber)
        {
            CallerFilePath = callerFilePath;
            LineNumber = lineNumber;
        }

        public override string ToString()
        {
            return $"{CallerFilePath}:line {LineNumber}";
        }

        public static CallerReference Create(string callerFilePath, int lineNumber)
        {
            if (string.IsNullOrEmpty(callerFilePath))
            {
                return null;
            }
            
            return new CallerReference(callerFilePath, lineNumber);
        }
    }
}