using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunIntended.Tools
{
    public class StateMachine<TOwner>
    {
        public TOwner Owner { get; private set; }

        private readonly Dictionary<Type, IState<TOwner>> _availableStates = new();

        private readonly List<IState<TOwner>> _currentStates = new();

        public StateMachine(TOwner owner, params IState<TOwner>[] states)
        {
            Owner = owner;
            foreach (IState<TOwner> state in states) 
            {
                state.Owner = Owner;
                state.StateMachine = this;

                Type type = state.GetType();
                _availableStates.Add(type, state);
            }
        }

        public void Switch<T>() 
            where T : IState<TOwner>
        {
            SwitchCurrentStates(typeof(T));
        }

        public void Switch<T, U>() 
            where T : IState<TOwner>
            where U : IState<TOwner>
        {
            SwitchCurrentStates(typeof(T), typeof(U));
        }

        public void Switch<T, U, V>() 
            where T : IState<TOwner>
            where U : IState<TOwner>
            where V : IState<TOwner>
        {
            SwitchCurrentStates(typeof(T), typeof(U), typeof(V));
        }

        public void Add<T>()
            where T : IState<TOwner>
        {
            AddState(typeof(T));
        }

        public void Remove<T>()
            where T : IState<TOwner>
        {
            RemoveState(typeof(T));
        }

        private void AddState(Type stateType)
        {
            IState<TOwner> state = _availableStates[stateType];
            _currentStates.Add(state);
            state.Setup();
            state.OnEnter();
        }

        private void RemoveState(Type stateType)
        {
            IState<TOwner> state = _availableStates[stateType];
            _currentStates.Remove(state);
            state.OnExit();
            state.Cleanup();
        }

        private void SwitchCurrentStates(params Type[] stateTypes)
        {
            // exit old states
            foreach (Type state in _currentStates)
            {
                RemoveState(state);
            }

            foreach (Type state in stateTypes)
            {
                AddState(state);
            }
        }
    }

    public interface IState<TOwner>
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public abstract void Setup();
        public abstract void Cleanup();
        public abstract void OnEnter();
        public abstract void OnExit();
    }

    public abstract class RuntimeState<TOwner> : IState<TOwner> where TOwner : MonoBehaviour
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        protected GameObject GameObject => Owner.gameObject;
        
        private readonly List<Coroutine> _activeCoroutines = new();

        public void Setup()
        {
            UpdateManager.Singleton.OnUpdate += OnUpdate;
            UpdateManager.Singleton.OnFixedUpdate += OnFixedUpdate;
        }

        public void Cleanup()
        {
            UpdateManager.Singleton.OnUpdate -= OnUpdate;
            UpdateManager.Singleton.OnFixedUpdate -= OnFixedUpdate;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }

        // MonoBehaviour.StartCoroutine for state
        protected Coroutine StartLocalCoroutine(IEnumerator enumerator)
        {
            // start the coroutine
            Coroutine coroutine = Owner.StartCoroutine(enumerator);
            _activeCoroutines.Add(coroutine);

            // start coroutine that will remove the coroutine when its finished
            Owner.StartCoroutine(RemoveFromActiveCoroutinesCoroutine(coroutine));
            return coroutine;
        }

        // automatically remove coroutine from current coroutines once its finished on its own
        private IEnumerator RemoveFromActiveCoroutinesCoroutine(Coroutine coroutine)
        {
            yield return coroutine;
            if (coroutine != null)
            {
                _activeCoroutines.Remove(coroutine);
            }
        }

        // MonoBehaviour.StopCoroutine for state
        protected void StopLocalCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                if (_activeCoroutines.Contains(coroutine))
                {
                    _activeCoroutines.Remove(coroutine);
                }

                Owner.StopCoroutine(coroutine);
            }
        }

        // MonoBehaviour.StopAllCoroutines for state
        // can get called from state implementation, is always called when state exits
        protected void StopAllLocalCoroutines()
        {
            List<Coroutine> coroutinesToStop = new List<Coroutine>();
            foreach (Coroutine coroutine in _activeCoroutines)
            {
                coroutinesToStop.Add(coroutine);
            }

            foreach (Coroutine coroutine in coroutinesToStop)
            {
                StopLocalCoroutine(coroutine);
            }

            _activeCoroutines.Clear();
        }
    }
}