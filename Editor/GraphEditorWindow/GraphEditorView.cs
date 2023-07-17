using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace PunIntended.Tools.Editor
{
    public class GraphEditorView : GraphView
    {
        public GraphEditorView()
        {
            string path = PunIntendedToolsEditorPath.StyleSheets + "StyleSheet_GridBackground.uss";
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
            styleSheets.Add(styleSheet);

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground background = new();
            background.StretchToParentSize();
            Insert(0, background);
        }
    }
}