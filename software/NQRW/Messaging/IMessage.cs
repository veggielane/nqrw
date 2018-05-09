using System;

namespace NQRW.Messaging
{
    public interface IMessage
    {
        DateTime Timestamp { get; }

    }
    public interface IMessage<T>
    {
        DateTime Timestamp { get; }
        Func<T> Callback { get; }
    }

}
