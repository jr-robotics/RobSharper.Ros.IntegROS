namespace RosComponentTesting.ExpectationProcessing
{
    public interface IValidationRule
    {
        bool IsValid { get; }
        
        ValidationState ValidationState { get; }
        
        void Validate(ValidationContext context);
    }
}