namespace NQRW.Messaging
{
    public interface IHandle
    {
    }

    public interface IHandle<TMessage> : IHandle where TMessage : IMessage
    { 
        void Handle(TMessage message);
    }
}
