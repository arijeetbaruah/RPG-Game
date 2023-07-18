using Game.Config;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class CurrencyConfig : BaseMultiConfig<CurrencyDataConfig, CurrencyConfig>
    {
        
    }

    [System.Serializable]
    public class CurrencyDataConfig : IConfigData
    {
        public string ID => currencyID;

        public string currencyID;
        public string currencyName;
        public AssetReferenceSprite icon;
    }
}
