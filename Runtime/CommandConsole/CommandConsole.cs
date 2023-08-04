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

        internal readonly ConcurrentDictionary<string, MethodInfo> AvailableCommands = new();
        internal readonly List<CommandConsoleLine> CommandHistory = new();

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

        internal void WriteLine(string text, CommandConsoleLine.Type type = CommandConsoleLine.Type.Normal)
        {
            CommandConsoleLine commandConsoleLine = new(text, type);
            CommandHistory.Add(commandConsoleLine);
        }
    }

    internal class CommandConsoleLine
    {
        internal string Text { get; private set; }
        internal Type LineType { get; private set; }

        internal CommandConsoleLine(string text, Type type)
        {
            Text = text;
            LineType = type;
        }

        internal enum Type
        {
            Normal,
            Warning,
            Error
        }
    }

    internal class ConsoleCommand
    {
        internal readonly string Input;
        internal readonly MethodInfo MethodInfo;
        internal readonly object[] Parameters;
        internal readonly CommandConsoleLine ConsoleLine;

        private readonly bool _initializedSuccesfully;

        internal ConsoleCommand(string input)
        {
            Input = input;
            string[] commandAndParameters = Input.Split(' ');
            string commandName = commandAndParameters[0];
            if (CommandConsole.Singleton.AvailableCommands.TryGetValue(commandName, out MethodInfo))
            {
                int parameterCount = commandAndParameters.Length - 1;
                Parameters = new object[parameterCount];
                for (int i = 0; i < parameterCount; i++)
                {
                    Parameters[i] = commandAndParameters[i + 1];
                }

                _initializedSuccesfully = TryCastParametersForCommand();
            }
            else
            {
                CommandConsole.Singleton.WriteLine($"{Input} is not a valid command!", CommandConsoleLine.Type.Error);
            }
        }

        internal void Execute()
        {
            if (_initializedSuccesfully)
            {
                Type type = MethodInfo.DeclaringType;
                CommandAttribute commandAttribute = MethodInfo.GetCustomAttribute<CommandAttribute>();
                switch (commandAttribute.ExecutionType)
                {
                    case CommandExectionType.FindFirstObjectOfType:
                        {
                            Object instance = Object.FindFirstObjectByType(type);
                            if (instance != null)
                            {
                                MethodInfo.Invoke(instance, Parameters);
                            }
                            else
                            {
                                CommandConsole.Singleton.WriteLine($"no instance for {type.Name} was found!", CommandConsoleLine.Type.Warning);
                            }
                        }

                        break;
                    case CommandExectionType.FindAllObjectsOfType:
                        {
                            Object[] instances = Object.FindObjectsByType(type, FindObjectsSortMode.InstanceID);
                            if (instances.Length > 0)
                            {
                                foreach (Object instance in instances)
                                {
                                    MethodInfo.Invoke(instance, Parameters);
                                }
                            }
                            else
                            {
                                CommandConsole.Singleton.WriteLine($"no instances for {type.Name} were found!", CommandConsoleLine.Type.Warning);
                            }
                        }

                        break;
                }
            }
        }

        // returns true if casting is succesful
        private bool TryCastParametersForCommand()
        {
            int expectedParameterCount = MethodInfo.GetParameters().Length;
            if (MethodInfo.GetParameters().Length != Parameters.Length)
            {
                CommandConsole.Singleton.WriteLine($"{Parameters.Length} parameters provided, expected {expectedParameterCount}!", CommandConsoleLine.Type.Error);
                return false;
            }

            ParameterInfo[] parameterInfos = MethodInfo.GetParameters();
            for (int i = 0; i < MethodInfo.GetParameters().Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                Type parameterType = parameterInfo.ParameterType;

                // ints
                if (parameterType == typeof(int))
                {
                    if (int.TryParse((string)Parameters[i], out int integer))
                    {
                        Parameters[i] = integer;
                    }
                    else
                    {
                        CommandConsole.Singleton.WriteLine($"{(string)Parameters[i]} was provided as parameter, but expected an integer!", CommandConsoleLine.Type.Error);
                        return false;
                    }
                }
                // floats
                else if (parameterType == typeof(float))
                {
                    if (float.TryParse((string)Parameters[i], out float floatingPoint))
                    {
                        Parameters[i] = floatingPoint;
                    }
                    else
                    {
                        CommandConsole.Singleton.WriteLine($"{(string)Parameters[i]} was provided as parameter, but expected a float!", CommandConsoleLine.Type.Error);
                        return false;
                    }
                }
                // bools
                else if (parameterType == typeof(bool))
                {
                    if ((string)Parameters[i] == "true")
                    {
                        Parameters[i] = true;
                    }
                    else if ((string)Parameters[i] == "false")
                    {
                        Parameters[i] = false;
                    }
                    else
                    {
                        CommandConsole.Singleton.WriteLine($"{(string)Parameters[i]} was provided as parameter, but expected a bool!", CommandConsoleLine.Type.Error);
                        return false;
                    }
                }
                // strings
                else if (parameterType == typeof(string))
                {
                    // Parameters if filled with strings by default, so no need to cast
                }
                else
                {
                    CommandConsole.Singleton.WriteLine($"parameter {(string)Parameters[i]} is of an unexpected type!", CommandConsoleLine.Type.Error);
                    return false;
                }
            }

            return true;
        }
    }
}