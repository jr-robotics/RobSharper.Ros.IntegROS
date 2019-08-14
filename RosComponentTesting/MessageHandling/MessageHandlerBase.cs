using RosComponentTesting.Debugging;

namespace RosComponentTesting.MessageHandling
{
    public abstract class MessageHandlerBase<TTopic>
    {
        public bool IsActive { get; private set; }

        protected MessageHandlerBase()
        {
        }

        public virtual void Activate()
        {
            IsActive = true;
        }

        public virtual void Deactivate()
        {
            IsActive = false;
        }

        public void HandleMessage(TTopic message, MessageHandlingContext context)
        {
            if (!IsActive)
                return;

            HandleMessageInternal(message, context);
        }
        
        protected abstract void HandleMessageInternal(TTopic message, MessageHandlingContext context);
    }

    public abstract class ValidationMessageHandlerBase<TTopic> : MessageHandlerBase<TTopic>, IValidationMessageHandler
    {
        public CallerReference CallerInfo { get;}
        
        protected ValidationMessageHandlerBase()
        {
        }
        
        protected ValidationMessageHandlerBase(CallerReference callerInfo)
        {
            CallerInfo = callerInfo;
        }

        public abstract bool IsValid { get; }
        public abstract ValidationState ValidationState { get; }
        public abstract void Validate(ValidationContext context);
    }
}