using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace PunIntended.Tools.Editor
{
    public class GraphEditorNode : GraphElement
    {
        private readonly GraphEditorWindow _graphEditorWindow;
        private readonly VisualElement _box;
        private readonly ListView _listView;

        private List<GameObject> _gameObjects = new List<GameObject>();

        public GraphEditorNode(GraphEditorWindow window, string title, Rect rect)
        {
            _graphEditorWindow = window;
            _box = new VisualElement();
            _box.style.backgroundColor = new StyleColor(Color.grey);
            _box.style.borderTopColor = new StyleColor(Color.white);
            _box.style.borderTopWidth = 10f;
            _box.style.width = rect.width;
            _box.style.height = rect.height;

            Label titleTabel = new(title);
            titleTabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _box.Add(titleTabel);
            Add(_box);

            const int itemCount = 1000;
            var items = new List<string>(itemCount);
            for (int i = 1; i <= itemCount; i++)
                items.Add(i.ToString());

            // The "makeItem" function is called when the
            // ListView needs more items to render.
            Func<VisualElement> makeItem = () => new Label();

            // As the user scrolls through the list, the ListView object
            // recycles elements created by the "makeItem" function,
            // and invoke the "bindItem" callback to associate
            // the element with the matching data item (specified as an index in the list).
            Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];

            // Provide the list view with an explicit height for every row
            // so it can calculate how many items to actually display
            const int itemHeight = 16;

            var listView = new ListView(items, itemHeight, makeItem, bindItem);

            listView.selectionType = SelectionType.Multiple;

            listView.itemsChosen += objects => Debug.Log(objects);
            listView.selectionChanged += objects => Debug.Log(objects);

            listView.style.flexGrow = 1.0f;

            _box.Add(listView);

            // Add the list view to the box
            _box.Add(_listView);

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