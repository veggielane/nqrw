using NQRW.Gait;
using NQRW.Messaging;

namespace NQRW.FiniteStateMachine
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
