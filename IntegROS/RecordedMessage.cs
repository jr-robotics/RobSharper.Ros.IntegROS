using System;

namespace IntegROS
{
    public class RecordedMessage<TType> : IRecordedMessage<TType> where TType : class
    {
        private readonly IRecordedMessage _innerMessage;
        private TType _message;

        public RecordedMessage(IRecordedMessage message)
        {
            _innerMessage = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Topic => _innerMessage.Topic;
        public TType Message
        {
            get
            {
                if (_message == null)
                {
                    _message = _innerMessage.GetMessage<TType>(); 
                }

                return _message;
            }
        }
    }
}