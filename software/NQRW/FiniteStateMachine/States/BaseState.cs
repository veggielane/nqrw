using System;
using System.Collections.Generic;
using NQRW.Messaging;

namespace NQRW.FiniteStateMachine.States
{
    public abstract class BaseState : IState
    {
        public string Name { get; }
        public IMessageBus Bus { get; }

        protected List<IDisposable> Subscriptions;

        public BaseState(string name, IMessageBus bus)
        {
            Name = name;
            Bus = bus;
            Subscriptions = new List<IDisposable>();
        }

        protected void Sub(IDisposable sub)
        {
            Subscriptions.Add(sub);
        }

        public virtual void Start()
        {
            Bus.System("Starting State: " + Name);
        }

        public virtual void Stop()
        {
            Bus.System("Stopping State: " + Name);
            foreach (var sub in Subscriptions)
            {
                sub?.Dispose();
            }
        }
    }
}
