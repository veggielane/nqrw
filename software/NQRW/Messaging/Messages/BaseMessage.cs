using System;

namespace NQRW.Messaging.Messages
{
    public abstract class BaseMessage : IMessage
    {
        public DateTime Timestamp { get; private set; }
        protected BaseMessage()
        {
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
