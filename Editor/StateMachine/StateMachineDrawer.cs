using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace PunIntended.Tools.Editor
{
    //[CustomPropertyDrawer(typeof(StateMachine<>))]
    public class StateMachineDrawer : PropertyDrawer
    {
        
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            StateMachine<MonoBehaviour> stateMachine = (StateMachine<MonoBehaviour>)property.objectReferenceValue;
            Debug.Log(stateMachine);
            // Now you can access the AvailableStates dictionary.
            //Dictionary<Type, IState<MonoBehaviour>> availableStates = stateMachine.AvailableStates;
            
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

            var listView = new ListView(items, itemHeight, makeItem, bindItem)
            {
                // Enables multiple selection using shift or ctrl/cmd keys.
                selectionType = SelectionType.Multiple,
                showAddRemoveFooter = true,
                reorderMode = ListViewReorderMode.Animated,
                showBorder = true,
                showFoldoutHeader = true,
                showBoundCollectionSize = false,
                showAlternatingRowBackgrounds = AlternatingRowBackground.None,

            };

            // Single click triggers "selectionChanged" with the selected items. (f.k.a. "onSelectionChange")
            // Use "selectedIndicesChanged" to get the indices of the selected items instead. (f.k.a. "onSelectedIndicesChange")
            listView.selectionChanged += objects => Debug.Log($"Selected: {string.Join(", ", objects)}");

            // Double-click triggers "itemsChosen" with the selected items. (f.k.a. "onItemsChosen")
            listView.itemsChosen += objects => Debug.Log($"Double-clicked: {string.Join(", ", objects)}");

            listView.style.flexGrow = 1.0f;
            return listView;
        }
        
        
    }
}