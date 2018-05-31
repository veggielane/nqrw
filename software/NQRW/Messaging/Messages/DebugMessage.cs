using System;

namespace NQRW.Messaging.Messages
{
    public class DebugMessage : BaseMessage
    {
        public string Message { get; private set; }
        public DebugMessage(String message)
        {
            Message = message;
        }
        public override string ToString()
        {
            return $"{Timestamp}: {Message}";
        }
    }
}