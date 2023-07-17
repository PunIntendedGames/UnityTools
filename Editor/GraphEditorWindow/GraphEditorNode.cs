using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace PunIntended.Tools.Editor
{
    public class GraphEditorNode : GraphElement
    {
        private Label _titleLabel;
        private VisualElement _box;

        public GraphEditorNode(string title, Rect rect)
        {
            _box = new VisualElement();
            _box.style.backgroundColor = new StyleColor(Color.grey);
            _box.style.borderTopColor = new StyleColor(Color.white);
            _box.style.borderTopWidth = 10f;
            _box.style.width = rect.width;
            _box.style.height = rect.height;

            _titleLabel = new Label(title);
            _titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

            _box.Add(_titleLabel);

            Add(_box);

            capabilities |= Capabilities.Selectable | Capabilities.Movable | Capabilities.Deletable | Capabilities.Ascendable | Capabilities.Copiable | Capabilities.Snappable | Capabilities.Groupable;
            usageHints = UsageHints.DynamicTransform;
            pickingMode = PickingMode.Position;
            SetPosition(rect);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            _box.style.borderTopColor = new StyleColor(Color.red);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            _box.style.borderTopColor = new StyleColor(Color.white);
        }
    }
}