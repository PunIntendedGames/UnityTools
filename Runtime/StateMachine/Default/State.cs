using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunIntended.Tools
{
    public abstract class State<TOwner> : IState<TOwner, State<TOwner>> where TOwner : MonoBehaviour
    {
        public TOwner Owner { get; set; }
        public StateMachineAbstract<TOwner, State<TOwner>> StateMachine { get; set; }
        protected GameObject GameObject => Owner.gameObject;
        protected Transform Transform => Owner.transform;

        private readonly List<Coroutine> _activeCoroutines = new();

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnGUI() { }

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