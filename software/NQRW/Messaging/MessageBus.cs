using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace NQRW.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly Subject<IMessage> _subject;
        public IObservable<IMessage> Messages { get; private set; }

        public MessageBus()
        {
            _subject = new Subject<IMessage>();
            Messages = _subject.AsObservable();
        }

        public void Add(IMessage message)
        {
            _subject.OnNext(message);
        }

        public void Debug(string message)
        {
            Add(new DebugMessage(message));
        }
    }


}
