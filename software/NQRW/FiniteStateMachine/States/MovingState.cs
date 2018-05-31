using System;
using System.Reactive.Linq;
using NQRW.FiniteStateMachine.Commands;
using NQRW.Gait;
using NQRW.Messaging;
using NQRW.Messaging.Messages;

namespace NQRW.FiniteStateMachine.States
{
    public class MovingState : BaseState
    {
        private readonly IGaitEngine _gaitEngine;
        private IDisposable _sub;

        public MovingState(IMessageBus bus, IGaitEngine gaitEngine) : base("Moving", bus)
        {
            _gaitEngine = gaitEngine;
        }
        public override void Start()
        {
            base.Start();
            _sub = Bus.Messages.OfType<HeadingEvent>().Subscribe(Check);
            _gaitEngine.Moving = true;
        }

        public override void Stop()
        {
            base.Stop();
            _sub.Dispose();
            _gaitEngine.Moving = false;

        }


        private void Check(HeadingEvent e)
        {
            if (e.Heading.Length < 0.1)
            {

                Bus.Add(new StopCommand());
            }

        }

    }
}
