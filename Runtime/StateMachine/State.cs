using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace PunIntended.Tools
{
    public interface IState<TOwner>
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public void OnSetup();
        public void OnCleanup();
        public void OnEnter();
        public void OnUpdate();
        public void OnFixedUpdate();
        public void OnLateUpdate();
        public void OnGUIUpdate();
        public void OnExit();
    }
    
    [Serializable]
    public abstract class State<TOwner> : IState<TOwner> where TOwner : MonoBehaviour
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public virtual void OnSetup() => StateUtility.SetupState(this);
        public virtual void OnCleanup() => StateUtility.CleanupState(this);
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnGUIUpdate() { }
        public virtual void OnExit() { }
    }

    [Serializable]
    public abstract class StateMonoBehaviour<TOwner> : MonoBehaviour, IState<TOwner> where TOwner : MonoBehaviour
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner> StateMachine { get; set; }
        public virtual void OnSetup() => StateUtility.SetupState(this);
        public virtual void OnCleanup() => StateUtility.CleanupState(this);
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnGUIUpdate() { }
        public virtual void OnExit() { }
    }

    public abstract class SuperState<TOwner> : StateMonoBehaviour<TOwner> where TOwner : MonoBehaviour
    {
        public List<IState<TOwner>> SubStates = new();

        public override void OnSetup()
        {
            foreach (IState<TOwner> state in SubStates)
            {
                StateUtility.SetupState(state);
            }
        }
        
        public override void OnCleanup()
        {
            foreach (IState<TOwner> state in SubStates)
            {
                StateUtility.CleanupState(state);
            }
        }
    }

    public static class StateUtility
    {
        public static void SetupState<TOwner>(IState<TOwner> state) where TOwner : MonoBehaviour
        {
            UnityEventManager.Singleton.Subscribe(state.OnUpdate, UnityEventManager.UpdateMethodType.Update, state.Owner);
            UnityEventManager.Singleton.Subscribe(state.OnFixedUpdate, UnityEventManager.UpdateMethodType.FixedUpdate, state.Owner);
            UnityEventManager.Singleton.Subscribe(state.OnLateUpdate, UnityEventManager.UpdateMethodType.LateUpdate, state.Owner);
            UnityEventManager.Singleton.Subscribe(state.OnGUIUpdate, UnityEventManager.UpdateMethodType.GUIUpdate, state.Owner);
        }

        public static void CleanupState<TOwner>(IState<TOwner> state) where TOwner : MonoBehaviour
        {
            UnityEventManager.Singleton.Unsubscribe(state.OnUpdate, UnityEventManager.UpdateMethodType.Update, state.Owner);
            UnityEventManager.Singleton.Unsubscribe(state.OnFixedUpdate, UnityEventManager.UpdateMethodType.FixedUpdate, state.Owner);
            UnityEventManager.Singleton.Unsubscribe(state.OnLateUpdate, UnityEventManager.UpdateMethodType.LateUpdate, state.Owner);
            UnityEventManager.Singleton.Unsubscribe(state.OnGUIUpdate, UnityEventManager.UpdateMethodType.GUIUpdate, state.Owner);
        }
    }
}