using NQRW.Messaging;

namespace NQRW.FiniteStateMachine
{
    public class IdleState : BaseState
    {
        public IdleState(IMessageBus bus) : base("Idle", bus)
        {
        }
    }
}
