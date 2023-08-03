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
            _commandHistory = new(Owner.CommandHistory, -1f, MakeItem, BindItem);
            background.Add(_commandHistory);

            _commandHistory.showBorder = true;
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
            // Create a VisualElement container to hold the label and the button
            VisualElement container = new();
            container.style.flexDirection = FlexDirection.Row; // Make the container a horizontal layout

            // Create the label
            Label label = new();
            label.style.flexGrow = 1; // The label will take all available space in the container
            container.Add(label);

            // Create the button
            Button button = new();
            button.text = "Invoke";
            button.style.width = 70f; // Set a fixed width for the button (you can change this value)
            container.Add(button);

            // Style the button and adjust its position within the container
            button.style.marginLeft = 10; // Adjust the space between the label and button

            return container;
        }

        private void BindItem(VisualElement element, int index)
        {
            Label label = element.Q<Label>();
            label.text = Owner.CommandHistory[index].Input;
        }

        private void OnClose()
        {
            StateMachine.Switch<CommandConsoleStateClosed>();
        }
    }
}