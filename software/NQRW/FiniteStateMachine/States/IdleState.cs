using NQRW.Messaging;

namespace NQRW.FiniteStateMachine.States
{
    public class IdleState : BaseState
    {
        public IdleState(IMessageBus bus) : base("Idle", bus)
        {
        }
    }
}
