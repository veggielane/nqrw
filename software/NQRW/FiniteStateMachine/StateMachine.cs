using NQRW.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NQRW.FiniteStateMachine.Commands;

namespace NQRW.FiniteStateMachine
{
    public class StateMachine : IStateMachine
    {
        public IMessageBus Bus { get; private set; }
        public IState Current { get; private set; }
        private readonly IList<IState> _states = new List<IState>();
        private readonly IDictionary<Type, IDictionary<Type, Type>> _transitions = new Dictionary<Type, IDictionary<Type, Type>>();//<IState, IDictionary<IStateCommand,IState>

        public StateMachine(IMessageBus bus)
        {
            Bus = bus;
            Bus.Messages.OfType<IStateCommand>().Subscribe(Next);
        }

        public void AddState(IState state)
        {
            _states.Add(state);
        }

        public void AddTransition<TFirstState, TStateCommand, TSecondState>()
            where TFirstState : IState
            where TStateCommand : IStateCommand
            where TSecondState : IState
        {
            if (_transitions.ContainsKey(typeof(TFirstState)))
            {
                _transitions[typeof(TFirstState)].Add(typeof(TStateCommand), typeof(TSecondState));
            }
            else
            {
                _transitions.Add(typeof(TFirstState), new Dictionary<Type, Type> { { typeof(TStateCommand), typeof(TSecondState) } });
            }
        }

        public void AddStates(params IState[] states)
        {
            foreach (var state in states)
            {
                AddState(state);
            }
        }

        public void Start<T>() where T : class, IState
        {

            var state = _states.SingleOrDefault(s => s.GetType() == typeof(T));
            if (state != null)
            {

                Current = state;
                Current.Start();
            }
        }

        public void Next<T>() where T : IStateCommand
        {
            Next(typeof(T));
        }

        public void Next(IStateCommand command)
        {
            Next(command.GetType());
        }

        private void Next(Type command)
        {
            if (_transitions.ContainsKey(Current.GetType()) && _transitions[Current.GetType()].ContainsKey(command))
            {
                var t = _transitions[Current.GetType()][command];
                var state = _states.SingleOrDefault(s => s.GetType() == t);
                if (state != null)
                {
                    Current.Stop();
                    Current = state;
                    Current.Start();
                }
                else
                {
                    Console.WriteLine("State Does not exist");
                }
            }
            else
            {
                Console.WriteLine("Invalid State Request");
            }
        }
    }
}
