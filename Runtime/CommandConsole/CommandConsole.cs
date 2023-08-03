using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace PunIntended.Tools
{
    public class CommandConsole : LazyMonoBehaviourSingleton<CommandConsole>
    {
        internal event Action OnToggleConsole;

        internal ConcurrentDictionary<string, MethodInfo> AvailableCommands = new();
        internal List<Command> CommandHistory = new();

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

        internal void WriteLine(string text)
        {
            Debug.Log(text);
        }

        internal void WriteWarning(string text)
        {
            Debug.Log(text);
        }

        internal void WriteError(string text)
        {
            Debug.Log(text);
        }

        internal void ExecuteCommand(Command command)
        {
            Type type = command.MethodInfo.DeclaringType;
            switch (command.Attribute.ExecutionType)
            {
                case CommandExectionType.FindFirstObjectOfType:
                    {
                        Object instance = FindFirstObjectByType(type);
                        if (instance != null)
                        {
                            command.MethodInfo.Invoke(instance, command.Parameters);
                        }
                        else
                        {
                            WriteWarning($"no instance for {type.Name} was found!");
                        }
                    }

                    break;
                case CommandExectionType.FindAllObjectsOfType:
                    {
                        Object[] instances = FindObjectsByType(type, FindObjectsSortMode.InstanceID);
                        if (instances.Length > 0)
                        {
                            foreach (Object instance in instances)
                            {
                                command.MethodInfo.Invoke(instance, command.Parameters);
                            }
                        }
                        else
                        {
                            WriteWarning($"no instances for {type.Name} were found!");
                        }
                    }

                    break;
            }
        }

        internal class Command
        {
            public string Input;
            public MethodInfo MethodInfo;
            public CommandAttribute Attribute;
            public object[] Parameters;
            public Color Color;
        }
    }
}