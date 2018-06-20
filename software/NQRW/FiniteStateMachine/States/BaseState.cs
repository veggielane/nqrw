using NQRW.Messaging;

namespace NQRW.FiniteStateMachine.States
{
    public abstract class BaseState : IState
    {
        public string Name { get; }
        public IMessageBus Bus { get; }

        public BaseState(string name, IMessageBus bus)
        {
            Name = name;
            Bus = bus;
        }

        public virtual void Start()
        {
            Bus.System("Starting State: " + Name);
        }

        public virtual void Stop()
        {
            Bus.System("Stopping State: " + Name);
        }
    }
}
