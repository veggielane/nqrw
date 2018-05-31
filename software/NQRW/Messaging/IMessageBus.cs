using System;

namespace NQRW.Messaging
{
    public interface IMessageBus
    {
        IObservable<IMessage> Messages { get; }
        void Add(IMessage message);
        void Debug(string message);
        void Debug<T>() where T : IMessage;
        void Handle(object o);
        void System(string message);
        void System<T>() where T : IMessage;
    }


}
