using UnityEditor;
using UnityEngine;

namespace PunIntended.Tools.Editor
{
    public class StateEditorWindow<TOwner> : IState<TOwner, StateEditorWindow<TOwner>> where TOwner : EditorWindow
    {
        public TOwner Owner { get; set; }
        public StateMachine<TOwner, StateEditorWindow<TOwner>> StateMachine { get; set; }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}