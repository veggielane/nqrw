using System;

namespace NQRW.Messaging
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
