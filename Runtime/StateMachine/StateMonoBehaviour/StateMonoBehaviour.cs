using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunIntended.Tools
{
    public class StateMonoBehaviour<TOwner> : MonoBehaviour, IState<TOwner, StateMonoBehaviour<TOwner>>
    {
        public TOwner Owner { get; set; }
        public StateMachineAbstract<TOwner, StateMonoBehaviour<TOwner>> StateMachine { get; set; }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnGUIUpdate() { }
    }
}
