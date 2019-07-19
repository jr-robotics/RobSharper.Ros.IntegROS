using System;

namespace RosComponentTesting
{
    public class TestExecutionOptions
    {
        private static TestExecutionOptions _defaultOptions = new TestExecutionOptions();

        public static TestExecutionOptions Default
        {
            get => _defaultOptions;
            set
            {
                if (value != null) _defaultOptions = value;
            }
        }

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
        public bool ContinueOnExpectationViolation { get; set; } = false;
    }
}