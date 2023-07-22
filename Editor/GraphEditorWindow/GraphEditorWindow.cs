using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace PunIntended.Tools.Editor
 {
    public class GraphEditorWindow : EditorWindow
    {
        private GraphEditorView _graphEditorView;
        private Toolbar _toolbar;
        private ContextualMenuManipulator _contextMenuManipulator;

        [MenuItem("Tools/Graph")]
        public static void OpenWindow()
        {
            GraphEditorWindow window = GetWindow<GraphEditorWindow>();
            window.titleContent = new GUIContent("Graph");
        }

        private void CreateGUI()
        {
            _graphEditorView = new GraphEditorView()
            {
                name = "Graph View"
            };

            _graphEditorView.StretchToParentSize();

            _toolbar = new();
            Button button = new();
            button.clicked += () => {
                CreateNode();
                button.Blur();
            };

            button.text = "Create Node";

            _toolbar.Add(button);

            rootVisualElement.Add(_graphEditorView);
            rootVisualElement.Add(_toolbar);

            _contextMenuManipulator = new ContextualMenuManipulator(ContextualMenuCallback);
            _graphEditorView.AddManipulator(_contextMenuManipulator);
        }

        private void ContextualMenuCallback(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is GraphEditorView)
            {
                evt.menu.AppendAction("Create Node", OnCreateNode);
            }
        }

        private void OnCreateNode(DropdownMenuAction action)
        {
            GraphEditorNode newNode = new GraphEditorNode(this, "New Node", new Rect(
                action.eventInfo.localMousePosition.x,
                action.eventInfo.localMousePosition.y, 
                200f, 
                200f));

            _graphEditorView.AddElement(newNode);
        }

        private void CreateNode()
        {
            GraphEditorNode node = new(this, "a node", new Rect(200f, 200f, 200f, 200f));
            _graphEditorView.AddElement(node);
        }

        public void RemoveNode(GraphEditorNode node)
        {
            _graphEditorView.RemoveElement(node);
        }
    }
}
