using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateOpened : State<CommandConsole>
    {
        private readonly Input_CommandConsole _commandConsoleInput = new();

        private TextField _inputField;
        private ListView _commandHistory;

        public override void OnEnter()
        {
            Owner.OnToggleConsole += OnClose;
            InitializeVisualElements();
            _commandConsoleInput.Enable();
            _commandConsoleInput.CommandConsole.Accept.performed += OnAccept;
        }

        public override void OnExit()
        {
            Owner.OnToggleConsole -= OnClose;
            _commandConsoleInput.Disable();
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
            _inputField = new();
            background.Add(_inputField);
            _inputField.style.position = Position.Relative;
            //textField.style.marginBottom = 400f;

            //Owner.ConsoleUIDocument.rootVisualElement.Add(textField);
            _inputField.Focus();

            // list of preview commands
            _commandHistory = new(Owner.CommandHistory, 30f, MakeItem, BindItem);
            background.Add(_commandHistory);
            _commandHistory.showBorder = true;

            //for (int i = 0; i < 20; i++)
            //{
            //    Owner.CommandHistory.Add(new CommandConsole.Command() { Input = "asdasdasd" + i * i});
            //}

            _commandHistory.Rebuild();
        }

        private void OnAccept(InputAction.CallbackContext context)
        {
            string input = _inputField.text;
            _inputField.value = string.Empty;
            Owner.CommandHistory.Insert(0, new CommandConsole.Command() { Input = input });
            _inputField.Focus();
            _commandHistory.Rebuild();
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