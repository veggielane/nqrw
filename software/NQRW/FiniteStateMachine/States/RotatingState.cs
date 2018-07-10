using System;
using System.Reactive.Linq;
using NQRW.FiniteStateMachine.Commands;
using NQRW.Gait;
using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;

namespace NQRW.FiniteStateMachine.States
{
    public class RotatingState : BaseState
    {
        private readonly IGaitEngine _gaitEngine;

        public RotatingState(IMessageBus bus, IGaitEngine gaitEngine) : base("Rotating", bus)
        {
            _gaitEngine = gaitEngine;
        }
        public override void Start()
        {
            base.Start();
            Sub(Bus.Messages.OfType<RotateEvent>().Subscribe(OnNext));
            _gaitEngine.Mode = WalkMode.Rotating;
        }

        public override void Stop()
        {
            base.Stop();
            _gaitEngine.Stop();
        }

        private void OnNext(RotateEvent e)
        {
            if (Math.Abs(e.Magnitude) < 0.1)
            {
                Bus.Add(new StopCommand());
            }
            else
            {
                _gaitEngine.Rotation = Angle.FromDegrees(e.Magnitude);
            }
        }
    }
}