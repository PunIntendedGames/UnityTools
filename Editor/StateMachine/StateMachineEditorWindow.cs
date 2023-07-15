using UnityEngine;
using UnityEditor;

namespace PunIntended.Tools.Editor
{
    public class StateMachineEditorWindow<TOwner> : StateMachine<TOwner, StateEditorWindow<TOwner>> where TOwner : EditorWindow
    {
        public StateMachineEditorWindow(TOwner owner, params StateEditorWindow<TOwner>[] states) : base(owner, states) { }
    }
}
