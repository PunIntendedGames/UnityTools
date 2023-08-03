using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateOpened : State<CommandConsole>
    {
        public override void OnEnter()
        {
            Owner.OnToggleConsole += OnClose;
            InitializeVisualElements();
        }

        public override void OnExit()
        {
            Owner.OnToggleConsole -= OnClose;
        }

        private void InitializeVisualElements()
        {
            // background
            VisualElement background = new();
            background.style.backgroundColor = new StyleColor(Color.grey);
            background.style.width = 500f;
            background.style.height = 400f;
            background.style.marginLeft = 20f;
            background.style.marginTop = 20f;
            background.style.alignContent = Align.Center;
            Owner.ConsoleUIDocument.rootVisualElement.Add(background);

            // command input text field
            TextField textField = new();
            background.Add(textField);
            textField.style.position = Position.Relative;
            //textField.style.marginBottom = 400f;

            //Owner.ConsoleUIDocument.rootVisualElement.Add(textField);
            textField.Focus();

            // list of preview commands
            ListView commandHistory = new(Owner.CommandHistory, 30f, MakeItem, BindItem);
            background.Add(commandHistory);
            commandHistory.showBorder = true;

            //for (int i = 0; i < 20; i++)
            //{
            //    Owner.CommandHistory.Add(new CommandConsole.Command() { Input = "asdasdasd" + i * i});
            //}

            commandHistory.Rebuild();
        }

        private VisualElement MakeItem()
        {
            Label label = new();
            return label;
        }

        private void BindItem(VisualElement element, int index)
        {
            var label = (Label)element;
            label.text = Owner.CommandHistory[index].Input;
        }

        private void OnClose()
        {
            StateMachine.Switch<CommandConsoleStateClosed>();
        }
    }
}