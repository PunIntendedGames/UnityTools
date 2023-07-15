using System;
using System.Collections.Generic;

namespace PunIntended.Tools
{
    public abstract class StateMachine<TOwner, TState> where TState : IState<TOwner, TState>
    {
        public TOwner Owner { get; private set; }

        private readonly Dictionary<Type, TState> _availableStates = new();

        protected readonly List<TState> CurrentStates = new();

        public StateMachine(TOwner owner, params TState[] states)
        {
            Owner = owner;
            foreach (TState state in states)
            {
                state.Owner = Owner;
                state.StateMachine = this;

                Type type = state.GetType();
                _availableStates.Add(type, state);
            }
        }

        // switching
        public void Switch<T>() 
            where T : IState<TOwner, TState>
        {
            SwitchCurrentStates(typeof(T));
        }

        public void Switch<T, U>() 
            where T : IState<TOwner, TState>
            where U : IState<TOwner, TState>
        {
            SwitchCurrentStates(typeof(T), typeof(U));
        }

        public void Switch<T, U, V>() 
            where T : IState<TOwner, TState>
            where U : IState<TOwner, TState>
            where V : IState<TOwner, TState>
        {
            SwitchCurrentStates(typeof(T), typeof(U), typeof(V));
        }

        private void SwitchCurrentStates(params Type[] stateTypes)
        {
            foreach (IState<TOwner, TState> stateType in CurrentStates)
            {
                RemoveCurrentState(stateType.GetType());
            }

            foreach (Type stateType in stateTypes)
            {
                AddCurrentState(stateType);
            }
        }

        // adding
        public void Add<T>()
            where T : IState<TOwner, TState>
        {
            AddCurrentState(typeof(T));
        }

        public void Add<T, U>()
            where T : IState<TOwner, TState>
        {
            AddCurrentStates(typeof(T), typeof(U));
        }

        public void Add<T, U, V>()
            where T : IState<TOwner, TState>
        {
            AddCurrentStates(typeof(T), typeof(U), typeof(V));
        }

        private void AddCurrentState(Type stateType)
        {
            TState state = _availableStates[stateType];
            CurrentStates.Add(state);
            state.OnEnter();
        }

        private void AddCurrentStates(params Type[] stateTypes)
        {
            foreach (Type stateType in stateTypes)
            {
                AddCurrentState(stateType);
            }
        }

        // removing
        public void Remove<T>()
            where T : IState<TOwner, TState>
        {
            RemoveCurrentState(typeof(T));
        }

        public void Remove<T, U>()
            where T : IState<TOwner, TState>
        {
            RemoveCurrentStates(typeof(T), typeof(U));
        }

        public void Remove<T, U, V>()
            where T : IState<TOwner, TState>
        {
            RemoveCurrentStates(typeof(T), typeof(U), typeof(V));
        }

        private void RemoveCurrentState(Type stateType)
        {
            TState state = _availableStates[stateType];
            CurrentStates.Remove(state);
            state.OnExit();
        }

        private void RemoveCurrentStates(params Type[] stateTypes)
        {
            foreach (Type stateType in stateTypes)
            {
                RemoveCurrentState(stateType);
            }
        }
    }
}