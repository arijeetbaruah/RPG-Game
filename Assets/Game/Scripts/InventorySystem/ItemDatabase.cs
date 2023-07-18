using Game.Config;
using UnityEngine;

namespace Game
{
    public class ItemDatabase : BaseMultiConfig<ItemData, ItemDatabase>
    {
        
    }

    [System.Serializable]
    public class ItemData : IConfigData
    {
        public string ID => itemID;

        public string itemID;
        public string itemName;
        [TextArea] public string itemDescription;

    }
}
