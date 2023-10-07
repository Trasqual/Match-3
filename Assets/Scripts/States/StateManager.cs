using System;
using System.Collections.Generic;
using System.Linq;

namespace GamePlay.StateMachine
{
    public class StateManager
    {
        public Action<State> OnStateChanged;
        public State CurrentState { get; private set; }

        private List<State> _states = new();

        public void AddState(State state)
        {
            if (_states.Contains(state)) return;
            _states.Add(state);
        }

        private bool HasState(Type state)
        {
            return _states.Any(x => x.GetType() == state);
        }

        public void ChangeState(Type state)
        {
            if (!HasState(state)) return;
            if (CurrentState.GetType() == state) return;

            CurrentState = _states.FirstOrDefault(x => x.GetType() == state);

            OnStateChanged?.Invoke(CurrentState);
        }
    }
}