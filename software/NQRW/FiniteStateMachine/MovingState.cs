using NQRW.Messaging;

namespace NQRW.FiniteStateMachine
{
    public class MovingState : BaseState
    {
        public MovingState(IMessageBus bus) : base("Moving", bus)
        {
        }
    }
}
