using System;
using System.Collections.Concurrent;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    public class CommandConsole : LazyMonoBehaviourSingleton<CommandConsole>
    {
        internal event Action OnToggleConsole;

        internal ConcurrentDictionary<string, MethodInfo> AvailableCommands = new();
        internal UIDocument ConsoleUIDocument;

        private StateMachine<CommandConsole> _stateMachine;

        private void Awake()
        {
            _stateMachine = new(
                this,
                new CommandConsoleStateUninitialized(),
                new CommandConsoleStateInitialize(),
                new CommandConsoleStateOpened(),
                new CommandConsoleStateClosed());

            _stateMachine.Switch<CommandConsoleStateUninitialized>();
        }

        public void Toggle()
        {
            OnToggleConsole?.Invoke();
        }

        public void WriteLine(string line)
        {
            Debug.Log(line);
        }
    }
}