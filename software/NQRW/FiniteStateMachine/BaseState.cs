﻿using NQRW.Messaging;

namespace NQRW.FiniteStateMachine
{
    public abstract class BaseState : IState
    {
        public string Name { get; private set; }
        public IMessageBus Bus { get; }

        public BaseState(string name, IMessageBus bus)
        {
            Name = name;
            Bus = bus;
        }

        public void Start()
        {
            Bus.Debug("Starting State: " + Name);
        }

        public void Stop()
        {
            Bus.Debug("Stopping State: " + Name);
        }
    }
}