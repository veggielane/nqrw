using System;
using System.Collections.Generic;
using NQRW.Messaging;

namespace NQRW.FiniteStateMachine.States
{
    public abstract class BaseState : IState
    {
        public string Name { get; }
        public IMessageBus Bus { get; }

        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

        public BaseState(string name, IMessageBus bus)
        {
            Name = name;
            Bus = bus;
        }

        protected void Sub(IDisposable sub)
        {
            _subscriptions.Add(sub);
        }

        public virtual void Start()
        {
            Bus.System(">>> State: " + Name);
        }

        public virtual void Stop()
        {
            Bus.System("<<< State: " + Name);
            foreach (var sub in _subscriptions)
            {
                sub?.Dispose();
            }
        }
    }
}
