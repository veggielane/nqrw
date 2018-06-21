using NQRW.Devices;
using NQRW.Messaging;

namespace NQRW.FiniteStateMachine.States
{
    public class IdleState : BaseState
    {
        private readonly IServoController _servoController;

        public IdleState(IMessageBus bus, IServoController servoController) : base("Idle", bus)
        {
            _servoController = servoController;
        }

        public override void Start()
        {
            base.Start();
            _servoController.Stop();
        }
    }
}
