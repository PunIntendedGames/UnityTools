using System;
using System.Collections.Generic;

namespace PunIntended.Tools
{
    [Serializable]
    public class StateMachine<TOwner> : UnityEngine.Object
    {
        public TOwner Owner { get; private set; }

        public readonly Dictionary<Type, IState<TOwner>> AvailableStates = new();

        private readonly List<IState<TOwner>> _currentStates = new();
        public IReadOnlyList<IState<TOwner>> CurrentStates => _currentStates;

        public StateMachine(TOwner owner, params IState<TOwner>[] states)
        {
            Owner = owner;
            foreach (IState<TOwner> state in states)
            {
                state.Owner = Owner;
                state.StateMachine = this;

                Type type = state.GetType();
                AvailableStates.Add(type, state);
            }
        }

        // switching
        public void Switch<T>()
            where T : IState<TOwner>
        {
            Switch(typeof(T));
        }

        public void Switch<T, U>()
            where T : IState<TOwner>
            where U : IState<TOwner>
        {
            Switch(typeof(T), typeof(U));
        }

        public void Switch<T, U, V>()
            where T : IState<TOwner>
            where U : IState<TOwner>
            where V : IState<TOwner>
        {
            Switch(typeof(T), typeof(U), typeof(V));
        }
        
        public void Switch(params Type[] stateTypes)
        {
            // need to get types first to avoid modifying current states collection
            List<Type> stateTypesToRemove = new();
            foreach (IState<TOwner> state in _currentStates)
            {
                stateTypesToRemove.Add(state.GetType());
            }

            foreach (Type stateType in stateTypesToRemove)
            {
                RemoveCurrentState(stateType);
            }

            foreach (Type stateType in stateTypes)
            {
                AddCurrentState(stateType);
            }
        }

        // adding
        public void Add<T>()
            where T : IState<TOwner>
        {
            AddCurrentState(typeof(T));
        }

        public void Add<T, U>()
            where T : IState<TOwner>
        {
            AddCurrentStates(typeof(T), typeof(U));
        }

        public void Add<T, U, V>()
            where T : IState<TOwner>
        {
            AddCurrentStates(typeof(T), typeof(U), typeof(V));
        }

        private void AddCurrentState(Type stateType)
        {
            IState<TOwner> state = AvailableStates[stateType];
            _currentStates.Add(state);
            state.OnSetup();
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
            where T : IState<TOwner>
        {
            RemoveCurrentState(typeof(T));
        }

        public void Remove<T, U>()
            where T : IState<TOwner>
        {
            RemoveCurrentStates(typeof(T), typeof(U));
        }

        public void Remove<T, U, V>()
            where T : IState<TOwner>
        {
            RemoveCurrentStates(typeof(T), typeof(U), typeof(V));
        }

        private void RemoveCurrentState(Type stateType)
        {
            IState<TOwner> state = AvailableStates[stateType];
            _currentStates.Remove(state);
            state.OnExit();
            state.OnCleanup();
        }

        private void RemoveCurrentStates(params Type[] stateTypes)
        {
            foreach (Type stateType in stateTypes)
            {
                RemoveCurrentState(stateType);
            }
        }

        public bool IsInState<T>()
            where T : IState<TOwner>
        {
            foreach (IState<TOwner> state in _currentStates)
            {
                if (state.GetType() == typeof(T))
                {
                    return true;
                }
            }

            return false;
        }
    }
}