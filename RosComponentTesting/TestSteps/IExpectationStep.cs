namespace RosComponentTesting.TestSteps
{
    public interface IExpectationStep
    {
        IExpectation Expectation { get; }
    }
}