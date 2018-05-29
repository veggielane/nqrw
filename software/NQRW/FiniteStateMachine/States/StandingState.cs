using NQRW.Messaging;

namespace NQRW.FiniteStateMachine.States
{
    public class StandingState : BaseState
    {
        public StandingState(IMessageBus bus) : base("Standing", bus)
        {
        }
    }
}
