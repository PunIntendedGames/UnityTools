using System.Collections.Generic;
using UnityEngine;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace PunIntended.Tools
{
    [CreateAssetMenu(fileName = "Assembly Definitions", menuName = "PunIntended/Tools/CommandConsole/Assembly Definitions")]
    public class AssemblyDefinitionsInUnityProjectSO : ScriptableObject
    {
        public List<string> AssemblyDefinitionNames = new();

#if UNITY_EDITOR
        public static bool Editor_CacheAssemblyDefinitionsEditor()
        {
            string typeName = nameof(AssemblyDefinitionsInUnityProjectSO);
            string[] guids = AssetDatabase.FindAssets($"t:{typeName}");
            if (guids.Length != 1)
            {
                StringBuilder foundSOs = new();
                for (int i = 0; i < guids.Length; i++)
                {
                    string foundSoGuid = guids[i];
                    string foundSoPath = AssetDatabase.GUIDToAssetPath(foundSoGuid);
                    foundSOs.Append(foundSoPath);
                    if (i < guids.Length - 1)
                    {
                        foundSOs.Append(", ");
                    }
                }

                Debug.LogError($"cashing of assembly definitons failed. {guids.Length} assembly definition in unity project scriptable objects were found, there must only be 1. locations of found scriptable objects: {foundSOs}");
                return false;
            }

            string guid = guids[0];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AssemblyDefinitionsInUnityProjectSO asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionsInUnityProjectSO>(path);
            asset.Editor_FindAndCacheAssemblyDefinitionsEditor();
            return true;
        }

        public int Editor_FindAndCacheAssemblyDefinitionsEditor()
        {
            AssemblyDefinitionNames.Clear();
            
            string filter = "t:" + nameof(AssemblyDefinitionAsset);
            string[] folders = { "Assets" };
            string[] guids = AssetDatabase.FindAssets(filter, folders);
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                string name = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path).name;
                AssemblyDefinitionNames.Add(name);
            }

            return guids.Length;
        }
#endif
    }
}