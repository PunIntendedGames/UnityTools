using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateInitialize : State<CommandConsole>
    {
        private const string _assemblyDefinitionsAssetPath = "Packages/com.punintended.tools/Runtime/CommandConsole/Assembly Definitions.asset";

        public override void OnEnter()
        {
#if UNITY_EDITOR
            Owner.WriteLine("fetching assemblies...");
            if (AssemblyDefinitionsInUnityProjectSO.Editor_CacheAssemblyDefinitionsEditor())
            {
                Owner.WriteLine("fetching assemblies succesful");
            }
            else
            {
                Owner.WriteLine("fetching assemblies unsuccesful");
            }
#endif
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Assembly> filteredAssemblies = new();
            Addressables.LoadAssetAsync<ScriptableObject>(_assemblyDefinitionsAssetPath).Completed += (handle) =>
            {
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
                {
                    Debug.LogError("failed loading assembly definitions asset from " + _assemblyDefinitionsAssetPath);
                    return;
                }

                // filter through assemblies
                AssemblyDefinitionsInUnityProjectSO asset = handle.Result as AssemblyDefinitionsInUnityProjectSO;
                foreach (Assembly assembly in assemblies)
                {
                    bool userWrittenAssembly = false;
                    foreach (string name in asset.AssemblyDefinitionNames)
                    {
                        if (name == (assembly.GetName().Name))
                        {
                            userWrittenAssembly = true;
                        }
                    }

                    if (userWrittenAssembly)
                    {
                        filteredAssemblies.Add(assembly);
                    }
                }

                // find command methods
                Parallel.ForEach(filteredAssemblies, assembly =>
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        foreach (MethodInfo method in type.GetMethods(
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance |
                            BindingFlags.Static))
                        {
                            CommandAttribute command = method.GetCustomAttribute<CommandAttribute>();
                            if (command != null)
                            {
                                string key = command.Name == string.Empty ? method.Name : command.Name;
                                if (!Owner.AvailableCommands.TryAdd(key, method))
                                {
                                    Owner.WriteLine($"command with name '{key}' has already been found!");
                                }
                            }
                        }
                    }
                });

                Owner.ConsoleUIDocument = Owner.gameObject.AddComponent<UIDocument>();

                StateMachine.Switch<CommandConsoleStateOpened>();
            };
        }
    }
}