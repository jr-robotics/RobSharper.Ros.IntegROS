namespace RosComponentTesting.MessageHandling
{
    public interface IValidationMessageHandler
    {
        bool IsValid { get; }
        
        ValidationState ValidationState { get; }
        
        void Validate(ValidationContext context);
    }
}