using System;

namespace NQRW.Messaging.Messages
{
    public class SystemMessage : BaseMessage
    {
        public string Message { get; private set; }
        public SystemMessage(String message)
        {
            Message = message;
        }
        public override string ToString()
        {
            return $"{Timestamp}: {Message}";
        }
    }
}
