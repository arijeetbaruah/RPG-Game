using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Game.Config
{
    [CreateAssetMenu(menuName = "Game/Config Registry")]
    public class ConfigRegistry : ScriptableObject
    {
        [SerializeField, FolderPath]
        private string configPaths;

        [SerializeField, Searchable, ShowInInspector, InlineEditor]
        private List<ScriptableObject> configRegistry = new List<ScriptableObject>();

        private Dictionary<Type, ScriptableObject> registry;

        public void Initialize()
        {
            registry = configRegistry.ToDictionary(config => config.GetType());
        }

        public TConfig Get<TConfig>() where TConfig : class, IConfig
        {
            Type type = typeof(TConfig);

            if (registry.TryGetValue(type, out ScriptableObject configSO))
            {
                return configSO as TConfig;
            }

            return null;
        }


        [Button]
        public void Refresh()
        {
#if UNITY_EDITOR
            System.Type configType = typeof(IConfig);
            Dictionary<Type, ScriptableObject> registry = configRegistry.ToDictionary(cr => cr.GetType());
            
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes()
                    .Where(config => configType.IsAssignableFrom(config) && !config.IsAbstract && !config.IsInterface).ToList())
                {
                    string configPath = Path.Combine(configPaths, $"{type.Name}.asset");

                    if (registry.ContainsKey(type))
                    {
                        continue;
                    }

                    ScriptableObject so = null;
                    
                    if (File.Exists(configPath))
                    {
                        so = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(configPath);
                    }
                    else
                    {
                        so = ScriptableObject.CreateInstance(type);
                        UnityEditor.AssetDatabase.CreateAsset(so, configPath);
                    }
                    configRegistry.Add(so);
                    UnityEditor.EditorUtility.SetDirty(so);
                    UnityEditor.AssetDatabase.SaveAssets();
                }
            }
#endif
        }
    }
}
