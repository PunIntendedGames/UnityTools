using UnityEngine;

namespace PunIntended.Tools
{
    public interface IState<TOwner>
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnLateUpdate();
        public abstract void OnGUIUpdate();
        public abstract void OnExit();
    }

    public abstract class State<TOwner> : IState<TOwner> where TOwner : MonoBehaviour
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnGUIUpdate() { }
        public virtual void OnExit() { }
    }

    public abstract class StateMonoBehaviour<TOwner> : MonoBehaviour, IState<TOwner> where TOwner : MonoBehaviour
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnGUIUpdate() { }
        public virtual void OnExit() { }
    }
}