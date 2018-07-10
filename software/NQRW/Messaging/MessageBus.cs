using NQRW.Messaging.Messages;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using JetBrains.Annotations;
using NQRW.Messaging.Logging;

namespace NQRW.Messaging
{
    [UsedImplicitly]
    public class MessageBus : IMessageBus
    {
        private readonly ILogger _logger;
        private readonly Subject<IMessage> _subject;
        public IObservable<IMessage> Messages { get; private set; }

        public MessageBus(ILogger logger)
        {
            _logger = logger;
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

        public void Debug<T>() where T : IMessage
        {
            Messages.OfType<T>().Subscribe(m => {
                Add(new DebugMessage(m.ToString()));
            });
        }

        public void Handle(object o)
        {
            foreach (Type intType in o.GetType().GetInterfaces())
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IHandle<>))
                {
                    Type t = intType.GetGenericArguments()[0];
                    Messages.Where(m => m.GetType() == t).Subscribe(m => o.GetType().GetMethod("Handle", new[] { t }).Invoke(o,new object[] { m }));
                }
            }
        }

        public void System(string message)
        {
            Add(new SystemMessage(message));
        }

        public void System<T>() where T : IMessage
        {
            Messages.OfType<T>().Subscribe(m => {
                Add(new SystemMessage(m.ToString()));
            });
        }

        public void Log<T>() where T : IMessage
        {
            Messages.OfType<T>().Subscribe(m => {
                _logger.Log(m.ToString());
            });
        }
    }
}
