using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace NQRW.Messaging
{
    public abstract class BaseMessage : IMessage
    {
        public DateTime Timestamp { get; private set; }
        protected BaseMessage()
        {
            Timestamp = DateTime.Now;
        }
    }


}
