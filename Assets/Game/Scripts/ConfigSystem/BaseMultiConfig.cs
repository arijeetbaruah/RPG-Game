using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Config
{
    public abstract class BaseMultiConfig<TConfigData, TConfig> : ScriptableObject, IConfig
        where TConfig : IConfig
        where TConfigData : IConfigData
    {
        public string ID => nameof(TConfig);
        public Dictionary<string, TConfigData> Data => registry;

        private Dictionary<string, TConfigData> registry;
        [SerializeField]
        private List<TConfigData> data;

        public void Initialize()
        {
            registry = data.ToDictionary(d => d.ID);
        }
    }
}
