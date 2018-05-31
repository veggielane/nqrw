using NQRW.Messaging;
using NQRW.Messaging.Messages;
using System;
using System.Reactive.Linq;
using NQRW.FiniteStateMachine.Commands;

namespace NQRW.FiniteStateMachine.States
{
    public class StandingState : BaseState
    {
        private IDisposable _sub;
        public StandingState(IMessageBus bus) : base("Standing", bus)
        {

        }

        public override void Start()
        {
            base.Start();
            _sub = Bus.Messages.OfType<HeadingEvent>().Subscribe(Check);
        }

        private void Check(HeadingEvent e)
        {
            if (e.Heading.Length > 0.1)
            {
                
                Bus.Add(new MoveCommand());
            }

        }

        public override void Stop()
        {
            _sub.Dispose();
            base.Stop();
        }
    }
}
