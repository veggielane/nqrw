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

        public MovingState(IMessageBus bus, IGaitEngine gaitEngine) : base("Moving", bus)
        {
            _gaitEngine = gaitEngine;
        }
        public override void Start()
        {
            base.Start();
            Sub(Bus.Messages.OfType<HeadingEvent>().Subscribe(OnNext));
            _gaitEngine.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _gaitEngine.Stop();
        }
        
        private void OnNext(HeadingEvent e)
        {
            if (e.Heading.Length < 0.1)
            {
                Bus.Add(new StopCommand());
            }
            else
            {
                _gaitEngine.Heading = e.Heading;
            }
        }
    }
}
