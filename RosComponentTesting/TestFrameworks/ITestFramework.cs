namespace RosComponentTesting.TestFrameworks
{
    public interface ITestFramework
    {
        bool IsLoaded { get; }

        void Throw(string errorMessage);
    }
}