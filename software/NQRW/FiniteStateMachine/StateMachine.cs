using NQRW.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using JetBrains.Annotations;
using NQRW.FiniteStateMachine.Commands;

namespace NQRW.FiniteStateMachine
{
    [UsedImplicitly]
    public class StateMachine : IStateMachine
    {
        private object _lock = new object();
        public IState Current { get; private set; }
        private readonly List<IState> _states = new List<IState>();
        private readonly IDictionary<Type, IDictionary<Type, Type>> _transitions = new Dictionary<Type, IDictionary<Type, Type>>();

        public StateMachine(IMessageBus bus, IEnumerable<IState> states)
        {
            bus.Messages.OfType<IStateCommand>().Subscribe(command => Next(command.GetType()));
            _states.AddRange(states);
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

        private void Next(Type command)
        {
            lock (_lock)
            {
                if (_transitions.ContainsKey(Current.GetType()) && _transitions[Current.GetType()].ContainsKey(command))
                {
                    var state = _states.SingleOrDefault(s => s.GetType() == _transitions[Current.GetType()][command]);
                    if (state != null)
                    {
                        Current.Stop();
                        Current = state;
                        Current.Start();
                    }
                    else throw new Exception("State Does not exist");
                }
            }
        }
    }
}
