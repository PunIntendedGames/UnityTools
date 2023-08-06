﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateOpened : State<CommandConsole>
    {
        private readonly Input_CommandConsole _commandConsoleInput = new();

        private TextField _inputField;
        private ListView _commandHistoryListView;

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
            _commandHistoryListView = new(Owner.CommandHistory, -1f, MakeItem, BindItem);
            background.Add(_commandHistoryListView);

            _commandHistoryListView.showBorder = true;
            _commandHistoryListView.Rebuild();
        }

        private void OnAccept(InputAction.CallbackContext context)
        {
            if (!string.IsNullOrWhiteSpace(_inputField.text))
            {
                string input = _inputField.text;
                _inputField.value = string.Empty;

                ConsoleCommand command = new(input);
                command.Execute();

                _commandHistoryListView.Rebuild();
            }

            _inputField.Focus();
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

            {
                // Create the button
                Button button = new();
                button.text = "Invoke";
                button.style.width = 70f;
                container.Add(button);
                button.style.marginLeft = 2f;
            }


            return container;
        }

        private void BindItem(VisualElement element, int index)
        {
            CommandConsoleLine line = Owner.CommandHistory[index];
            Label label = element.Q<Label>();
            label.text = line.Text;
            label.style.color = GetColor(line.LineType);
        }

        private void OnClose()
        {
            StateMachine.Switch<CommandConsoleStateClosed>();
        }

        private static Color GetColor(CommandConsoleLine.Type type)
        {
            return type switch
            {
                CommandConsoleLine.Type.Normal => Color.white,
                CommandConsoleLine.Type.Warning => Color.yellow,
                CommandConsoleLine.Type.Error => Color.red,
                _ => Color.magenta, // should not happen!
            };
        }
    }
}