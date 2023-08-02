using UnityEngine;

namespace PunIntended.Tools
{
    public interface IState<TOwner>
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public abstract void OnSetup();
        public abstract void OnCleanup();
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
        public void OnSetup() => StateUtility.SetupState(this);
        public void OnCleanup() => StateUtility.CleanupState(this);
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
        public void OnSetup() => StateUtility.SetupState(this);
        public void OnCleanup() => StateUtility.CleanupState(this);
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnGUIUpdate() { }
        public virtual void OnExit() { }
    }

    internal static class StateUtility
    {
        internal static void SetupState<TOwner>(IState<TOwner> state) where TOwner : MonoBehaviour
        {
            UnityEventManager.Singleton.Subscribe(state.OnUpdate, UnityEventManager.UpdateMethodType.Update, state.Owner);
            UnityEventManager.Singleton.Subscribe(state.OnFixedUpdate, UnityEventManager.UpdateMethodType.FixedUpdate, state.Owner);
            UnityEventManager.Singleton.Subscribe(state.OnLateUpdate, UnityEventManager.UpdateMethodType.LateUpdate, state.Owner);
            UnityEventManager.Singleton.Subscribe(state.OnGUIUpdate, UnityEventManager.UpdateMethodType.GUIUpdate, state.Owner);
        }

        internal static void CleanupState<TOwner>(IState<TOwner> state) where TOwner : MonoBehaviour
        {
            UnityEventManager.Singleton.Unsubscribe(state.OnUpdate, UnityEventManager.UpdateMethodType.Update, state.Owner);
            UnityEventManager.Singleton.Unsubscribe(state.OnFixedUpdate, UnityEventManager.UpdateMethodType.FixedUpdate, state.Owner);
            UnityEventManager.Singleton.Unsubscribe(state.OnLateUpdate, UnityEventManager.UpdateMethodType.LateUpdate, state.Owner);
            UnityEventManager.Singleton.Unsubscribe(state.OnGUIUpdate, UnityEventManager.UpdateMethodType.GUIUpdate, state.Owner);
        }
    }
}