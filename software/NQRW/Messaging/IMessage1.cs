using System;

namespace NQRW.Messaging
{
    public interface IMessage<T>
    {
        DateTime Timestamp { get; }
        Func<T> Callback { get; }
    }


}
