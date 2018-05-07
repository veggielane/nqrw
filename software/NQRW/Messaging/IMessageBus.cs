using System;

namespace NQRW.Messaging
{
    public interface IMessageBus
    {
        IObservable<IMessage> Messages { get; }
        void Add(IMessage message);
        void Debug(string message);
    }


}
