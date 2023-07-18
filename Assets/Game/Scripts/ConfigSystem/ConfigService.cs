using Game.Service;
using UnityEngine.AddressableAssets;

namespace Game.Config
{
    public class ConfigService : IService
    {
        private ConfigRegistry configRegistry;

        public async void Initialize()
        {
            var hander = Addressables.LoadAssetAsync<ConfigRegistry>(nameof(ConfigRegistry));
            configRegistry = await hander.Task;

            UnityEngine.Debug.Log("ConfigRegistry loaded");
        }

        public void Release()
        {

        }

        public void Update()
        {

        }

        public TConfig Get<TConfig>() where TConfig : class, IConfig
        {
            return configRegistry.Get<TConfig>();
        }
    }
}
