using NQRW.Gait;
using NQRW.Messaging;
using NQRW.Robotics;
using NQRW.Timing;
using System;

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
            _gaitEngine.Moving = true;
        }

        public override void Stop()
        {
            base.Stop();
            _gaitEngine.Moving = false;
        }
    }
}
