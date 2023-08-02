using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace PunIntended.Tools
{
    internal class CommandConsoleStateInitialize : State<CommandConsole>
    {
        private const string _assemblyDefinitionsAssetPath = "Packages/com.punintended.tools/Runtime/CommandConsole/Assembly Definitions.asset";
        private const string _panelSettingsPath = "Packages/com.punintended.tools/Runtime/CommandConsole/PanelSettings_CommandConsole.asset";
        private const string _uxmlPath = "Packages/com.punintended.tools/Runtime/CommandConsole/UXML_CommandConsole.uxml";

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
                if (handle.Status == AsyncOperationStatus.Failed)
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
                                string key = command.Alias == string.Empty ? method.Name : command.Alias;
                                if (!Owner.AvailableCommands.TryAdd(key, method))
                                {
                                    Owner.WriteWarning($"command with name '{key}' has already been found!");
                                }
                            }
                        }
                    }
                });

                foreach (Assembly assembly in filteredAssemblies)
                {
                    Owner.WriteLine(assembly.FullName);
                }

                Owner.ConsoleUIDocument = Owner.gameObject.AddComponent<UIDocument>();

                // TODO(berend): there is probably a better way to figure out if all assets have loaded.
                // we are also never unloading the assets, is this a problem if we want to keep them loaded
                // the entire lifetime of the application?
                Addressables.LoadAssetAsync<PanelSettings>(_panelSettingsPath).Completed += (handle) =>
                {
                    Owner.ConsoleUIDocument.panelSettings = handle.Result;
                    LoadedAsset();
                };

                Addressables.LoadAssetAsync<VisualTreeAsset>(_uxmlPath).Completed += (handle) =>
                {
                    Owner.ConsoleUIDocument.visualTreeAsset = handle.Result;
                    LoadedAsset();
                };
            };
        }

        private int _loadedAssetsCount;
        private void LoadedAsset()
        {
            _loadedAssetsCount++;
            if (_loadedAssetsCount >= 2)
            {
                StateMachine.Switch<CommandConsoleStateOpened>();
            }
        }
    }
}