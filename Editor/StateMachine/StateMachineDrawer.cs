using UnityEngine;
using UnityEditor;

namespace PunIntended.Tools.Editor
{
    [CustomPropertyDrawer(typeof(StateMachine<>))]
    public class StateMachineDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            GUILayout.Label("statemachine is here!");

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0f;
        }
    }
}