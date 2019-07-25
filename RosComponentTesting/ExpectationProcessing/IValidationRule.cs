namespace RosComponentTesting.ExpectationProcessing
{
    public interface IValidationRule
    {
        void Validate(ValidationContext context);
    }
}