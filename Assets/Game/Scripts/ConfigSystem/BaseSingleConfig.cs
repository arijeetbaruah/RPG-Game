using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Config
{
    public abstract class BaseSingleConfig<TConfigData, TConfig> : ScriptableObject, IConfig
        where TConfig : IConfig
        where TConfigData : IConfigData
    {
        public string ID => nameof(TConfig);
        public TConfigData Data => data;

        [SerializeField, InlineProperty, HideLabel]
        private TConfigData data;

        public void Initialize()
        {
            
        }
    }
}
