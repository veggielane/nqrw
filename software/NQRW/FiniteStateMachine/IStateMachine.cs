using NQRW.FiniteStateMachine.Commands;

namespace NQRW.FiniteStateMachine
{
    public interface IStateMachine
    {
        IState Current { get; }
        void AddTransition<TFirstState, TStateTransition, TSecondState>()
            where TFirstState : IState
            where TStateTransition : IStateCommand
            where TSecondState : IState;
        void Start<T>()
            where T : class, IState;
        void Next<T>() where T : IStateCommand;
    }
}
