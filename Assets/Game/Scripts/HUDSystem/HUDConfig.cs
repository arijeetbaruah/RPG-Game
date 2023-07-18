using Game.Config;

namespace Game.HUDSystem
{
    public class HUDConfig : BaseMultiConfig<HUDDataConfig, HUDConfig>
    {
        
    }

    [System.Serializable]
    public class HUDDataConfig : IConfigData
    {
        public string ID => HUDId;

        public string HUDId;
        public HUDAssetReference assetReference;
    }
}
